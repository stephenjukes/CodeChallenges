﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace NbsCodeChallenges.AdventOfCode2021
{
    public class Day_10
    {
        private static IEnumerable<Enclosure> _enclosures;

        public static void Run()
        {
            var input = Day10Input.Test;

            _enclosures = new Enclosure[]
            {
                new Enclosure('(', ')', corruptionPoints: 3, completionPoints: 1),
                new Enclosure('[', ']', corruptionPoints: 57, completionPoints: 2),
                new Enclosure('{', '}', corruptionPoints: 1197, completionPoints: 3),
                new Enclosure('<', '>', corruptionPoints: 25137, completionPoints: 4)
            };

            var validationResults = input.Select(CheckValidity);

            // Part 1
            var part1Solution = validationResults
                .Where(e => !e.IsValid && e.Reason == Reason.Corrupt)
                .Sum(result => _enclosures
                    .First(e => e.Closing.ToString() == result.ErrorValue).CorruptionPoints);

            Console.WriteLine($"Part 1 solution: { part1Solution }");


            // Part 2
            var incompleteEnclosures = validationResults
                .Where(result => !result.IsValid && result.Reason == Reason.Incomplete);

            long initialTotal = 0;
            
            var part2Solution = incompleteEnclosures
                .Select(result => string.Join("", result.ErrorValue.Reverse()))
                .Select(unclosed => unclosed
                    .Aggregate(initialTotal, (total, opening) => total * 5 + _enclosures.First(e => e.Opening == opening).CompletionPoints))
                .OrderBy(total => total)
                .Skip(incompleteEnclosures.Count() / 2).Take(1).First();

            Console.WriteLine($"Part 2 solution: { part2Solution }");
        }

        private static ValidationCheck CheckValidity(string line)
        {
            var openingEnclosures = new List<char>();

            foreach (var enclosure in line)
            {
                if (IsOpening(enclosure))
                {
                    openingEnclosures.Add(enclosure);
                    continue;
                }

                var closure = _enclosures.First(e => e.Closing == enclosure);

                if (!(openingEnclosures.Last() == closure.Opening))
                {
                    return new ValidationCheck(line, false, Reason.Corrupt, enclosure.ToString());
                }

                openingEnclosures.RemoveAt(openingEnclosures.Count - 1);
            }

            if (openingEnclosures.Any())
            {
                return new ValidationCheck(line, false, Reason.Incomplete, string.Join("", openingEnclosures));
            }

            return new ValidationCheck(line, true, Reason.NonApplicable, default);
        }

        private static bool IsOpening(char enclosure)
        {
            return _enclosures.Select(e => e.Opening).Contains(enclosure);
        }
    }

    public class Enclosure
    {
        public Enclosure(char opening, char closing, int corruptionPoints, int completionPoints)
        {
            Opening = opening;
            Closing = closing;
            CorruptionPoints = corruptionPoints;
            CompletionPoints = completionPoints;
        }

        public char Opening { get; set; }
        public char Closing { get; set; }
        public int CorruptionPoints { get; set; }
        public int CompletionPoints { get; set; }
    }

    public class ValidationCheck
    {
        public ValidationCheck(string line, bool isValid, Reason reason, string errorValue)
        {
            Line = line;
            IsValid = isValid;
            Reason = reason;
            ErrorValue = errorValue;
        }

        public string Line { get; set; }
        public bool IsValid { get; set; }
        public Reason Reason { get; set; }
        public string ErrorValue { get; set; }
    }

    public enum Reason
    {
        Corrupt,
        Incomplete,
        NonApplicable
    }

    public static class Day10Input
    {
        public static string[] Test = new string[]
        {
            "[({(<(())[]>[[{[]{<()<>>",
            "[(()[<>])]({[<{<<[]>>(",
            "{([(<{}[<>[]}>{[]{[(<()>",
            "(((({<>}<{<{<>}{[]{[]{}",
            "[[<[([]))<([[{}[[()]]]",
            "[{[{({}]{}}([{[{{{}}([]",
            "{<[[]]>}<{[{[{[]{()[[[]",
            "[<(<(<(<{}))><([]([]()",
            "<{([([[(<>()){}]>(<<{{",
            "<{([{{}}[<[[[<>{}]]]>[]]"
        };

        public static string[] Challenge = new string[]
        {
            "{<[[<<<[[{[[<<()()>>[([]<>){{}{}}]]]<(<{{}<>}{{}{}}><{<>[]}[{}{}]>)({(())}<[(){}][(){}]>)>}]<<<(<<{}",
            "{[({({([({(((<[]()>[()<>]){[<>[]](<>[])})<<<[]()}>(<()()>)>)}[{((<{}[]>{<>()}){[<>{}]<<><>>})(({[]()}<<>()>)<",
            "(((([{{{<{<[[<()<>><<>{}>]]{<(<>())>}>{<[([]<>)<{}{}>][[<><>][<>[]]]>[{(<>{})[<>()]}<<()[]>[[]<",
            "<<[<([[{{{[({<<>()>}<<<>[]>([]())>){{{{}[]}<(){}>)}]{{[(()())[(){}]]<<[]{}>({}[])>}({<<><>>}<([]<>){()[]}>)}",
            "<[(({[({<<[[[{<>{}}(<><>))<(<>[])[{}[]]>]<[([][])[(){}]]>]([<[()()](<>[])>]<(<{}()>[{}<>])({<>[",
            "(<[[{[[[(<((<<{}{}><[]<>>><[{}<>][<>[]]>)<(<{}<>><{}{}>)[{()[]}<<>()>]>)>[(<{({}{})}[{()[]}{[]<>}]>){<(([]",
            "(([[({{{(<([({[]{}}){{[][]]}])>)[<([[[{}]{(){}}][([]())]]([{[][]}{<>()}](({}()){<>{}})))[{({",
            "{(<<([([(<<[{((){})([]<>)}]<{{{}{}}{{}[]}}<{(){}}{()[]})>><([<[]{}>([]<>)](<()<>>)){<([]<>",
            "{[<<[<{{((({<[[]{}]{{}}>{<{}<>>[<>[]]}})<{[(<><>><{}<>>][{[]{}}]}[[[[][]]<<>{}>]{[[]()][[]<>]}]>)[(",
            "({<(<([{<{<[(((){}){(){}})]>}[{{{{()()}<[]()>}{{<>()}<{}{}>}}[[{{}{}}[<><>]]{({}[]>[[]{}]}]}({{{()()",
            "{[[{([{<<{<(<<{}()>[()]>)<{{()()}}[<[][]>(<>{})]>>{[<{<>()){()<>}>{{()()}[<>()]}][[[{}[]]]<{{}[]}<()(",
            "{{<<<<<[[([{<[{}[]]{()<>}>{<<>()>{{}<>}}}<<{{}()}{()()}>{[()<>]({}[])]>]{[(<{}{}>)<({}[])>]<{({}<>)}{({}())",
            "({(<<{[<<{<(<[<>()][[][]]><[{}<>]({}[])>)[[[<>[]][[]()]]<{[]{}}[{}{}]>]>{[(<[]()>{<>{}})][<{{}}<{",
            "(<<<[[({<[[(<<[]{}>>[(()<>)[<>{}]])<{(<>[])}(<<>()>[[]{}])>]<[({[]()}{{}{}})<{{}()}<{}<>>>]>][[({<<>()>{[]()}",
            "{{{[<[([<(<{[([][])[{}[]]]{[()()]{{}()}}}><[<([][])(<>())><{()[]}({}<>)>]<<<(){}><<><>>>((()()){",
            "[(((([{({<(<(<[]<>>{()[]})><{(<><>){{}[]}}{(()<>)([][])}>)>})}][({{[[{<(<><>){{}()}>((<>()){<>[]",
            "([{<{<({(<[(<({}<>)><{[]}[{}[]]>)(<[[][]]{[]{}}>{{[]}((){})})]{(((()){{}()})(([][])({}{})))[(<()[]><(",
            "[([{{<{(<[{{((()())<[]{}>){{{}<>}<<><>>}}[<([]())>[(()[])<{}{}>]]}{[({(){}}{(){}])<<[]()>{{}()}>",
            "(<[<{{[<<({<<{<><>}><<()[]>[<><>]>>}{[(((){}){()<>}}<<[]()>{(){}}>]{{{{}[]}[{}<>]}[{{}{}}<(){}>]}}){{({",
            "<([([[<[({<<{{[][]}<{}[]>}<<()[]>[()<>]>>>})((<(<[[]]((){})>{([]()){<><>}})>)(<<<<[]<>>[[]()])>>([[",
            "[<<{<(<<{{[({<<>()><{}[]>})<{[<>()](<><>)}[{{}()}{[]()}]>]}}({<[{<()<>>}[<<>>]]((<[][]>{[][",
            "{{(<({[((<([({()<>})[{{}[]}(()<>))]{[{[]<>}][{()<>}]})([({<><>}{{}()})])>)({<(((()<>)(<><>)))>((",
            "[(<{({[<{<[<([{}()](<>{}))>]([([[]<>]<()<>>){<()>}]{{[[]()]{{}{}}}[{[]}{{}<>}]})>}<([<[<<>[]>]{<(){}>{",
            "[[[[<(<({<[[<{()}{{}[]}><[<><>]<()[]>>](<([]{})[{}]>)]>}<[{[[{(){}}]{(<><>)}][<<[][]>{(){}",
            "{(({({[{{((<[([]())((){})]>){[{(<>[])}<([]{})[[]<>]>]({<()<>>[[]<>]}[<()>{<>[]}])})({<([[][]]((){}))([<>{}])",
            "<(<(<<{[<<{<<[()[]][[]()]><([][])<{}()>>}}>[<<<<{}<>>>{[{}][{}{}]}>[<[()()]{{}[]}>[{[][]}([])]]>[[<<[][]>(",
            "<<{{({<<(([{{([][])(()<>)}}(<<[]{}>>)])([{[(()[])]}<{{<><>}<[]<>>}[<{}<>><[]<>)]>]{{<[{}[]]",
            "({[[[[(([[[{<({})([]())>{{<>{}}}}][[<<[]>[<>()]>]{[[[]<>][<>{}]]{[<>[]]}})][[<(<(){}>{()[]})><[[()[",
            "(((((([{{{(<<(<>{}){<>}><(()[])<()[]>>>{[<{}<>>(<>())}<{{}()}>})((([(){}]{<>})[{[]{}}{{}[]",
            "(([([(((<[(({[<>()]{[][]}})[{{[]()>[[]{}]}(<<>{}><{}<>>)]){[({[]}(<>[]))([()()][<>{}])]({({}",
            "[(<({{([[<(((<<>()>[[]<>])({{}()}<()<>>))[[{[]()}<()<>>][{()()}<{}[]>]])>({(<({}{})<()[]}>[{()",
            "{{{[([<{{{([<<<>{}>{()<>}>{<{}()><<><>>}]([{()}]))<{[<()()>{()[]}]{({}[])[<>()]}}>}}}({<(({({",
            "[([([<[<(<[{{{<>()}{()[]}}({()[]}[{}<>])}{[{<>}]([[][]]<(){}>)}]>[[{<[<>()][{}()]>(((){}>[<>[]])}{{{<>(",
            "{{[{{[[<{(<([[[][]]([]<>)])>)<{<<{<>()}[<>()]><((){})<<><>>>>([<<><>>(()<>)]([(){}}{[]<>}))}>}(({{{{[]}{",
            "[[[[({[<({{[[(<><>)]{<<>[]>([]{})}]<[{[]}{()()}]([<>()]([]<>>)>}})>{{<([{<{}{}>{()}}])({<[<>()](<><>)>",
            "{[({<[({[{[({[{}()][[]<>]}((()<>)<{}<>>))<[{{}}({}{})]>]{[<<<>[]>[[][]]>(({}[])(()[]))>{(([]{})({}[]))([",
            "[<<([{{(<<[<[[{}[]]](<[]{}>[<>{}])>[[<{}[]>(<>[])]{([][])[()[]]}]][[{{()[]}[<>{}]}(<<>{}>[[]",
            "<[(([{([([<{[[(){}]<<>{}>]}[[[()()]{{}<>}]]>[[{({}{})[()()]}([()()][<>[]])]{<[[]<>][[][]]>}]]{{<{[<>",
            "[<[[[[([[{[[<[{}{}]([]<>)>[({}[])<[]<>>]][<{()()}[<>[]]>([[]]<<><>>)]]}([<{{{}[]}}{(()<>>({}[",
            "<[<{[(<{({[<[[[][]]([][])]<([]){{}<>}>>[({<>{}}<[]>)]]([[([]{})[[]()]][[[]{}]({}{})]](<[{}<>",
            "({[<([(<[[<([[(){}]<<>{}>])({[[]{}]<()[]>}<<[]<>>{[][]}>)>{<[<[]()><{}[]>]([<>()]({}[]))><(",
            "({<<<{<({[[{[{<>{}}[<><>]]}{(({})[()()])<<()<>>([]{})>}]]]{<{{([[]<>](<>[]))[[{}()]({}())]}}>((<([{}<>])>{{<[",
            "[{([{[[{{<{<[{[]()}<[]{}>]>({<{}[]>{<>[]}})}[(({{}<>}<{}<>]){[[]{}]({}[])})]>}}]({([[([<()()>{[]",
            "<[{([([{[{<(([<>[]]{<>()})<{{}<>}>){([<><>])<(()[])<<>{}>>}><{{[{}<>]{<>}}[(())[()<>]]}<{[{}{}]{{}{}}}<{{}<>",
            "[{<<([{{(((<{<<>()>[{}()]}>{<<<>[]><[]()>>}){[(([][])([]()))<{{}[]}[[]()]>][[{{}<>}({}{})]{({}{})(<",
            "{({[((<([[[{{([])[()()]}({{}{}}))<(<[]<>>[()[]])>]<<([[]<>])<[<>()][[]<>]>>({[<><>]{{}[]}}({()[]",
            "{[(<(<{[({[(<<[]>{<>{}}>[{<><>}([]())])(<[[][]]<()<>>>[(()[])])]<{[{()[]}[<>{}]]}[{<(){}>(<>{})}<<<>()>>]",
            "(<{<[<({{{{([({}{})[[]{}]]({{}<>}<()[]}))}(<<{<>()}{()<>}>{[<>{}][()[]]}>{<<[][]>{<>()}><(<>{})<{}[]>>})}",
            "{(<<{[({<<[(<{{}{}}<[]<>>>((<>())([]<>)))]>><<({<{[]<>}([]<>)>{[()()]}}<<{{}<>}<()()>>(<()()",
            "<([([<(<[({[[({}()){()<>}](({}{})(<><>))][{({}<>)<{}{}>}{([]){(){}}}]})]>)<[[{<{{(()){<>[]}}({{}()}",
            "<<<([{[({{{((<{}()>{()()})[[<>()]])[[[{}()]<{}()>]{{[]<>}{{}())}]}<<{<<>><<><>>}[{{}[]}(<><>)",
            "<([<[<{[[[{[{[<><>][{}<>]}(<[]{}>)]}([{{<>()}<{}[]>}{([]<>)[(){}]}]<{((){})<[]()>}[<[]()><()[]>]>",
            "[([[[[[({{[(<[{}<>]<{}()>><[<><>]{<>()}>)]}<{(<(()<>)<{}{}>>{{()<>}(()()}})}>})(<[<([<<>[]>((){})]([[]<>]<()",
            "{(<[[{(((([[([{}()]<{}()))[({}{}){[]}]](<<<>[]>>([<>{}]))][(<({}{})<()[]>><<[]{}><{}[]>>)])(<([([][])[",
            "({(([{{<{{{<[<<>[]>{()<>}][([][]){()<>}]>([<()>(<><>)]<{<><>}<[]>>)}<{[[<>[]][<>{}]]<{<>()}{[][]",
            "{[([[[(([<{<{[[]<>]<<>()>}<<()()>([]{})>>[<<()[]>(<>())>[(()<>)]]}>]{{({{{{}[]}({})}<{{}<>}[[]()]>}((",
            "[<([[<[{{{[[[<<>{}>]{<()[]>{[]<>}}]{([[]{}]({}()))}]}}}(<{<[[({}{})<()()>]]({({}<>)[[]]}{[<><>]{<>{}}})>(<[",
            "<<(<<{{(({<[<<[]{}>{()()}}[[()[]]]]{[(<>{}){<>}]{[{}](<>)}}><{<(()())<[]<>>>[<(){}>(<>[])]",
            "{<(<[<[(((<{((()[])([]<>))([[]<>]({}[]))}><<([<>())<[]<>>)(<{}[]>({}<>))>[[([]())]({()()}{{}<>})",
            "[[((<[[[{{<{<{{}()}[{}()]>{{[]{}}[<><>]}}>}<{[<[(){}]<<><>>]([{}<>]<<><>>)]}(<<[{}<>]<<>{}>",
            "<[{{[[{<[{(({[(){}]({}{})})[([{}<>][()()])[<[][]><{}<>>]])}](<({(([]<>){()[]})[([]())((){})]}{{{<>{}}<[]()>}<",
            "<{((<([[{<<[(<{}()><{}[]>)]>[([{<>()}<()()>]<{()[]><()[]>>){<[{}[]]>[{[][]}(<><>)]}]>{{[{{{}<>}{<>}}[[(){}]<(",
            "{<[({(<{{[([{{[]{}}((){})}[[{}[]]<{}()>]]((<[]<>>)<([]()){{}[]}>))(<<{()[]}{[]})>[[(()())]{[()<>][<>(",
            "<<{[[[[{<({<[[{}()]]{{[]<>}{[]}}><{{{}()}({}{})}>}{([({}[])({}())](({}[])))[<{[]<>}[<>()]><<[]<>>(<>[])>]}",
            "[(([<<[{[{<({{()[]}<<><>>})[[([]())[[]{}])(<<>{}>)]>(<{{[][]}[{}()]}([{}{}][(){}])>((<<>[]>(()",
            "[({<<[[<{[<<({[]()}<()<>>)>]]{{<[[[]][{}<>]]>}{([[<>{}][{}{}]]<((){})(()[])>)}}}([{((<{}[]>)[<{}[]><{}()>])}",
            "{[[((<[{<<([{<<><>>([]{})}<(<><>)<<>()>>][{[[]{}]{()()}}<({}{})([][])>])><<{({<>{}}[<>()])((",
            "[[[<{{(<((([{<[]{}>({}<>)}[<<>()>]]<<(()[]){{}[]}>{(()<>){{}<>}}>){{{(()())[<><>]}[[[]<>]{<>[]}]}}))[",
            "(<({[({[[({{<{<>{}}<()[]>>[{(){}}<<>[]>]}[[<[][]><()<>>]({<>{}}{[]()})]}{(([[]<>][{}[]])(<[]<>>([",
            "{([<{[<({{{({[[][]][()()])[<<>()><[][]>])}<{{{[]()}[{}()]}{{<>()}{<>}}}((({}())<{}{}>){(<>{})[<>{}]})>}})(((",
            "([[[[[[{(((((<<>()>[[]<>])<{()()}[()<>]>){<[()[]]<<>[]>>(<<>{}}{{}[]})}))<<[([()()][[]()])[{()[]}([]())]][<([",
            "({{((((<{<[{[(<>{})][<()()>[[]()]]}]{({(<>())[[]()]}{<[][]><[][]>})}>}>{<<{{(({}())[<>])((()[])<[]<>>)}}[",
            "<(<[[[<<{([<<[[]<>]{<>[]}><{()()}([]<>)>>]{<{(<>())((){})}{[{}{}]<<>{}>}>})({([[{}{}]])[(<<>()>[",
            "{[(<{<[[{<{{(<()[]><<>{}>){[()<>]<{}<>>}}[[[<>{}][[]<>]]]}<[[({})<<>[]>]]{{<()<>><<>[]>}<{()[]}((){}",
            "({[{(<<{[<{({[()()]{{}}}<({}{}){()()}>)}<{(({}{})[<>[]]){([]())[[][]]}}{(<[]<>)[()])[[<>()]]}>>",
            "(({<<[<[<<<({<{}[]>({}{})}{<{}[]><{}[]>})>>(<<(<()()>{[]{}})({()<>}[()()])>[{[{}()][[]]}({<>()}}]>(",
            "<<(<(<[{([{<[[<>{}]<{}<>>]>{<(()[])(()())><{<>[]}{()()}>}}([{({}<>){<>{}}}{{<>[]}[{}{}]}]([<[]()>((){})",
            "{[{([({[[<<<{{[]{}}<[]{}>}[<()[]><[]<>>]>([{{}[]][[][]]]{{(){}}({}[])})>(<{[<>{}](()<>)}>{[<[]()>",
            "([[[<(<{[({<({()()}{<>()})((<>[])<[]<>>)>}<{<<{}()><<>>>}({{{}[]}}[(()[])[()<>]])>)<[<((()())[<>{}])(",
            "{<(((<[((((({<[]>[[]{}]}))[(<[[][]][{}{}]>){{[<>()][[]<>]}([()()](<><>))}])){(<[{[<>()][{}()]",
            "({([{<(((<{[<[{}[]]<<>>>({{}[]}[[]()])]([<(){}>])}{<[{<>[]}{[]<>}][<<><>){<>{}}]>[<((){})([]{})>[[{}{}][()",
            "<[{[<[{[({{<(({}<>}[[]()])>({{[][]}}<[(){}](()[])>)}})([[{{<<>()>([]{})}[<<>{}>[<><>]]}]])]",
            "{(<{([{[[({[(<[]{}>)({{}[]}{()()}))<{(()[])}[[()()]{{}[]}]>}<{<(()<>)>{{[]{}}({}())}}({{{}[]}{<",
            "[<<{<{<<(<([<(<>()){[]()}>[({}<>)({}<>)]][(([]{}){()[]})[{{}()}(()())]])>{([{{()[]}<<>[]>}<{<>[",
            "{{[[(([([({([([]<>)(()())][{<>()}{(){}}])})])]([{{[<{{{}<>}(<>())>{[()()]<<>[]>}>[{(()){{}[",
            "[({({({(({{(<[()()][{}<>]>[[{}()]{(){}}])<[<<>[]>][<{}>(<>{})]>}(((({}())({}{}))){{(()[])<[][",
            "<(<(<[([<[{[<(()<>)<()[]>><(<>())([][])>]{<[{}<>]{{}()}>{[[]{}](<>{})}}}<[<([]{})><{{}<>}([]())>]>]>",
            "<<<(<<[({{{<{({}<>)<()[]>}<[()<>]{(){}}>><{[[]()]<[]{}>}[({}{})<(){}>]>}(<[<<>[]>{()[]}]>)}[<{[<{}>[{}{",
            "{(<((<[{{[[[[<{}{}]<{}()>]][{({}{})<<>{}>}[<()[]>(()())]]]{([<[]()><[]{}>](<{}()>))({<<><>>(<>())}{",
            "{({{[[<[{<{{(<<>>)(<[]()>[()])}([({}<>)])}[[{[()<>][()[]]}]({<[]<>>{<>()}})]><<(<<{}()>{<>[]",
            "{{{[{[<<{[[{[{{}<>}<[][]>]{(()[])<<>()>}}[<{()()}[(){}]><<()[]>>]]{<((<>{})({}()))[<(){}>([][])]>{({{}[]}<",
            "<{{[<{({{{[<{{<>()}{{}()}}>]<{<[()[]](<><>)><[<>[]]{(){}}>}<([(){}]<{}{}>){(())}>>}[([{[[]<>][{}<>]}[{()}(",
            "[<<[{{(({({<{{{}{}}{<>[]}}({[][]}{<>[]})>[{[{}<>][<>[]]}[({}[])[[]<>]]]})}))<(<[[<(({}()))[<<>()>{(",
            "{<<([<{({[{{[({}[])<<>{}>]([()<>]<(){}>)}<([{}[]]({}{}))>}[<<<<>{}>[[][]]>{<<>[]><()>}>[<<<>><[][]>",
            "[({(<({[<[{{[<<>[]>[()[]]]<<<>[]>>}}<{([{}[]](<>{}))[{<>}{{}<>}]}(<<[][]>>[{[]<>}])>]>[(<(({<>[]}{<><>}))(<",
            "{<[[{{<<{(<(<([]<>)[[][]]>[([]{})[<>{}]])<[{<>()}{[]<>}]>>>[{(({()()}[<>[]]){({}())<[]<>>})}[(<{<>",
            "{[[{{{<[<[{<(({}{})[{}()])[(<><>)({}())]>}{[<{[]{}}[()[]]>]<{[()()]<[]()>}<[[][]]>>}]>{{({{{()[]}{{}(",
            "<{[<([{{[[<(<{<><>}[{}{}]><((){})>){[[()()][()[]]]{[[]<>]}}>]<<(<({}[])<{}[]>>{(<><>)<[]()>}){(<"
        };
    } 
}
