const canvas = document.getElementById("canvas");
const ctx = canvas.getContext("2d");
ctx.font = "30px Arial";

const canvasHeight = Number(getComputedStyle(canvas).height.match(/\d+/));
const canvasWidth = Number(getComputedStyle(canvas).width.match(/\d+/));

const getSideQuantity = () => Number(document.getElementById("sides").value);
const getInternalAngle = sideQuantity => 2 * Math.PI / sideQuantity;    // this should be rotation or external angle
const getMidPoint = (point1, point2) => getCollinearPoint(point1, point2, 0.5);
const mod = (n, m) => ((n % m) + m) % m;

document.getElementById("button2").addEventListener("click", () => animateSutcliffePentagon(2, 0, 1));;
document.getElementById("button3").addEventListener("click", () => animateSutcliffePentagon(3, 0, 1));
document.getElementById("button4").addEventListener("click", () => animateSutcliffePentagon(4, 0.2, 0.2));
document.getElementById("button5").addEventListener("click", () => animateSutcliffePentagon(5, 0.2, 0.2));

class Coordinate {
    constructor(x, y, type = null) {
        this.type = type;
        this.x = x;
        this.y = y;
    }

    ofType (type) {
        this.type = type;
        return this;
    }
}

const coordinateType = {
    vertex: "vertex",
    extensionBase: "extensionBase",
    extensionTip: "extensionTip"
}

let interval;
function animateSutcliffePentagon(level, minExtenstion, maxExtension) {
    clearInterval(interval)

    const start = new Coordinate(canvasWidth / 2 , canvasHeight / 2);  
    const sideQuantity =  getSideQuantity();
    const padding = 100;
    const radius = 0.5 * canvasWidth - padding;
    let extensionProportion = minExtenstion;

    interval = setInterval(function() {
        ctx.clearRect(0, 0, canvas.width, canvas.height)
        ctx.fillText(`ExtensionProportion: ${extensionProportion.toFixed(2)}`, 10, 50);
        ctx.beginPath();
        
        extensionProportion += 0.005;
        generateSutcliffePentagon(start, sideQuantity, radius, extensionProportion, level);

        if (extensionProportion > maxExtension) clearInterval(interval);
    }, 30);
}

function generateSutcliffePentagon(start, sideQuantity, radius, extensionProportion, level) {
    const vertices = getVertices(start, sideQuantity, radius).map(vertex => vertex.ofType(coordinateType.vertex));
    joinCoordinatesInOrder(vertices);

    recurseSutcliffePentagon(vertices, extensionProportion, level);
}

function getVertices(start, sideQuantity, radius) {
    const internalAngle = getInternalAngle(sideQuantity);
    const shapeOrientationOffset = - Math.PI / 2 // upright
    
    return new Array(sideQuantity).fill(null)
        .map((e, i) => resolveDestination(start, internalAngle * i + shapeOrientationOffset, radius));
}

function resolveDestination(start, angle, length) {
    return new Coordinate(
        Math.round(start.x + length * Math.cos(angle)), 
        Math.round(start.y + length * Math.sin(angle))
    );
}

function recurseSutcliffePentagon(vertices, extensionProportion, level) {
    if (level === 0) return;

    const internalVertexGroups = getInternalVertexGroups(vertices, extensionProportion);

    internalVertexGroups.forDrawing.forEach(vertexGroup => joinCoordinatesInOrder(vertexGroup, level));
    internalVertexGroups.forRecursion.forEach(vertexGroup => recurseSutcliffePentagon(vertexGroup, extensionProportion, level - 1));
}

function getInternalVertexGroups(vertices, extensionProportion) {
    const extensions = getExtensions(vertices, extensionProportion);
    const externalGroups = getExternalGroups(vertices, extensions);
    const centralGroup = extensions.map(e => e.tip);

    return {
        forRecursion: externalGroups.concat([centralGroup]),
        forDrawing: [centralGroup].concat(extensions.map(e => Object.values(e)))
    }
}

function getExtensions(vertices, extensionProportion) {
    return vertices.map((vertex, i, array) => {
        const base = getMidPoint(vertex, array[mod(i - 1, array.length)]);
        const oppositeVertex = array[(i + Math.floor(array.length / 2)) % array.length];
        const tip = getCollinearPoint(base, oppositeVertex, extensionProportion).ofType(coordinateType.extensionTip);

        return {
            base: base,
            tip: tip
        }
    })
}

function getExternalGroups(vertices, extensions) {
    return  extensions.map((extension, i, array) => {
        const nextExtension = array[mod(i + 1, array.length)];
        return [vertices[i], extension.base, extension.tip, nextExtension.tip, nextExtension.base];
    })
}

function getCollinearPoint(point1, point2, proportionFrom1to2) {
    return new Coordinate(
        point1.x + proportionFrom1to2 * (point2.x - point1.x),
        point1.y + proportionFrom1to2 * (point2.y - point1.y),
    );
}

function joinCoordinatesInOrder(group, level) {
    const start = group[group.length - 1];
    ctx.moveTo(start.x, start.y);

    group.forEach((coordinate) => {
        ctx.lineTo(coordinate.x, coordinate.y);
        ctx.lineWidth = 0.006;  // level not having the intended effect
        ctx.stroke();
    });
}