using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NbsCodeChallenges.AdventOfCode2021
{
    public class Day_08
    {
        public static void Run()
        {
            var input = Day8Input.Challenge;

			// Part 1
            var outputValues = input.SelectMany(line => line.Split(" | ")[1].Split(" "));
            var solutionValues = outputValues.Where(value => value.Length == 2 || value.Length == 3 || value.Length == 4 || value.Length == 7);

			// Part 2
			var funcs = new Func<IEnumerable<string>, string[], Digit>[]
			{
				// order is important
				(digits, wires) => new Digit(1, digits.Single(d => d.Length == 2)),
				(digits, wires) => new Digit(4, digits.Single(d => d.Length == 4)),
				(digits, wires) => new Digit(7, digits.Single(d => d.Length == 3)),
				(digits, wires) => new Digit(8, digits.Single(d => d.Length == 7)),
				(digits, wires) => new Digit(9, digits.Single(d => d.Length == 6 && d.ContainsAllOf(wires[4]))),
				(digits, wires) => new Digit(3, digits.Single(d => d.Length == 5 && d.ContainsAllOf(wires[7]))),
				(digits, wires) => new Digit(5, digits.Single(d => d.Length == 5 && wires[9].ContainsAllOf(d))),
				(digits, wires) => new Digit(6, digits.Single(d => d.Length == 6 && d.ContainsAllOf(wires[5]))),
				(digits, wires) => new Digit(0, digits.Single(d => d.Length == 6)),
				(digits, wires) => new Digit(2, digits.Single(d => d.Length == 5)),
			};

			var total = 0;
			foreach (var line in input)
            {
                var parts = line.Split(" | ");
                var digits = parts[0].Split(" ").ToList();
                var output = parts[1].Split(" ");

				var wires = new string[10];
				foreach(var func in funcs)
                {
					var digit = func(digits, wires);
					
					wires[digit.Value] = digit.SignalWires;
					
					digits.Remove(digit.SignalWires);
                }

				wires = wires.Select(wire => SortString(wire)).ToArray();
				var lineArray = output.Select(digit => Array.IndexOf(wires, SortString(digit)));
				
				total += int.Parse(string.Join("", lineArray));
            }

            Console.WriteLine(total);
        }

		private static string SortString(string input)
		{
			char[] characters = input.ToArray();
			Array.Sort(characters);
			return new string(characters);
		}
	}

	public static class Day8Extensions
    {
		public static bool ContainsAllOf(this string a, string b) => b.All(character => a.Contains(character));
    }

	public class Digit
    {
        public Digit(int value, string signalWires)
        {
			Value = value;
			SignalWires = signalWires;
        }

        public int Value { get; set; }
        public string SignalWires { get; set; }
        public IEnumerable<string> Resolver { get; set; }

        public override string ToString()
        {
            return $"{Value}: {SignalWires}";
        }
    }

    public static class Day8Input
    {
        public static string[] Test = new string[]
        {
            "be cfbegad cbdgef fgaecd cgeb fdcge agebfd fecdb fabcd edb | fdgacbe cefdb cefbgd gcbe",
            "edbfga begcd cbg gc gcadebf fbgde acbgfd abcde gfcbed gfec | fcgedb cgb dgebacf gc",
            "fgaebd cg bdaec gdafb agbcfd gdcbef bgcad gfac gcb cdgabef | cg cg fdcagb cbg",
            "fbegcd cbd adcefb dageb afcb bc aefdc ecdab fgdeca fcdbega | efabcd cedba gadfec cb",
            "aecbfdg fbg gf bafeg dbefa fcge gcbea fcaegb dgceab fcbdga | gecf egdcabf bgf bfgea",
            "fgeab ca afcebg bdacfeg cfaedg gcfdb baec bfadeg bafgc acf | gebdcfa ecba ca fadegcb",
            "dbcfg fgd bdegcaf fgec aegbdf ecdfab fbedc dacgb gdcebf gf | cefg dcbef fcge gbcadfe",
            "bdfegc cbegaf gecbf dfcage bdacg ed bedf ced adcbefg gebcd | ed bcgafe cdgba cbgef",
            "egadfb cdbfeg cegd fecab cgb gbdefca cg fgcdab egfdb bfceg | gbdfcae bgc cg cgb",
            "gcafb gcf dcaebfg ecagb gf abcdeg gaef cafbge fdbac fegbdc | fgae cfgab fg bagce"
        };

		public static string[] Challenge = new string[]
		{
						"febca cfagb ecbafd efdcbg cbegdfa fg bgafec gfae acgdb gfc | cgf facdeb ecgfdb afcbge",
			"acbed dcagb cfbega ecgdab be deafc bfgdca abe edafgbc debg | dceab dgbca gedabc fecabgd",
			"decfb bgadec dgefa bcgfade cae adecf ca eacfdb bcaf dfgbec | bfca fdacbge cedfb dfebac",
			"egd agdcfe gedbfa gbcfae ed dafgebc bdcgf gdbfe fgabe dbae | cefbga egfcba dbfeg ed",
			"bfeca beadgc gdfeacb aebcfd eg aefg cgefab ebgfc gbdfc beg | bedacf eg dabegc bfeac",
			"fcd fd fgdceba edgac cegdbf gcafdb agdfc cafbg beagfc dfab | dacbfg gfacb fcd fd",
			"cfbgeda gdfae abdfg dfceg edagfb geba bfcaed fcdbag ae aef | dcbefa dagfb dcefg abgdef",
			"edafbg ebg gbdea cdbefga bfgcea ge egdf aedcb gfadb gbcfda | degbaf dbgafe ecdab dagbf",
			"adecfb aebdc ab efbagd cfbed cagde baecgfd egdbfc bad bcaf | cbaf beadfgc febcd gaecd",
			"gfdacb bgdace gb cdfbg fcbde fdgeac bgfa dgb acfgd bdcefag | bg dbegac fbdegac agfb",
			"bfdace bdcfa ebcag egcdbfa dbg dg fbegdc bcgda fgabdc gfad | cadgb abcdg eagbc dagf",
			"bed faegcb fedcba dgbcea feabc cdefb bd bcdfgea edgcf afdb | dgcfe adfebgc ebgcad cfebd",
			"cfead dc dagef gfdc cgdefa acdbeg cad egfabcd bgeafd aecfb | dafge fdgeba faecd cbaef",
			"afgbde gebca ecbdaf caedb faceg abgcedf cdbeag cbdg bg agb | bg bcaed aecgb cgbae",
			"dfab dfgae bcafge cbefagd fdbeg fa fea fcbdge aegcd bdeagf | gfaedb af acbgfe faecgb",
			"decafg cadef fadecb dfeba aecbgd gefbd aeb ba ebfgdca abfc | gbfaedc bdefg acbf gcafde",
			"efgbd efdacg cd gefcd becagf dgc edca egdbafc cegfa fgdbca | eacgf bgafcd bfaegc dc",
			"gabed gdacfe egabcdf cdaf fdg acgfe fgdea gafbce degbcf df | fcaeg cefag bfegdc dfg",
			"gd badgcf cdg gbdcae cbdea dgfceba bfadec gdbe geadc acefg | gdc ebdg dg aegcf",
			"gbae fedcag bg ecgbda bfecd dcage fecgbda bgdce acfdgb gbd | cbgafd edcfb dbg bedcg",
			"abcgdf cfgae cebag gedbafc dcgbae abg decfbg gcebd ab dabe | gdbeac ab ab dcfbga",
			"gab efgbd gbcfad edgfac ba gaced dgcbae edbag fcgedab aecb | gfdace adebg ba cgbdfa",
			"acebg aedbfg deca ad dcaebg cgfbd abd cdabg dbecgfa agfceb | ad fadegb dcae cbfgd",
			"fdagcbe ebgdac ecfgbd fbc fc cbged fbegc cdef cfdgba agbfe | bfgce begcf cdfe cbf",
			"cbd fadgb dceag cb gfadeb begfcd cafb cgfdba gbadc cegdbaf | bdc bc adcbfeg dgace",
			"bgdefca cbgadf ebacf dfbac gedcfa cef fe gcbae fbed fcdbae | fe cadbfe ebfgacd cdbafg",
			"acfde cgeabd cag gfcb adgfc fdgbae fbdag cgefabd dgbacf cg | fgadc dcbgae ebdagf gbacfed",
			"ebdcg cfdbage cefbag cb ecb dabc cagdeb gbefd afdcge adegc | dgbafec dcfgea dgbef aecdg",
			"acd bfcged aedgfcb afdbce efcdb cfdae cbae cdafgb gdefa ac | aceb ecbgfad bdaefc efbcd",
			"cebd aefdgcb bafecg bfagd adfecg dcgab gcd cd ebcadg cebag | cdg cdg gcdba dafgec",
			"aecgbf bgfae gbcfeda fba abce gebfcd ab gcebf gcdfab gdafe | begcf baf baf fbgcae",
			"acdgfb bgedfa cbadfe degf gbeac gd adg dbgcaef abged febda | dg dg ebdfca fgacbd",
			"cfb fbdgac gcdaefb dabec fbcda fagbd cf cdgf bacfge ebfagd | bcdae bgdeafc acegbdf cf",
			"caegdb acgbf abdcgf fgebd afcd cd ecafgbd gcbdf dgc fbgace | acfbg efacdbg bcfgd fbcgad",
			"eab fecbdg dcfab fdcgaeb gdebc cegadb bcaed bfgace ea aegd | ea bfadc bcgdae becfag",
			"cg fdbeg eafcb bcefda cabfdeg cbfgae bfecg bagc gec dfgaec | bagc ecg cg abcg",
			"ceafg dgeab ecd fcbedg edcgfa agdbfce dafc bcfage agcde cd | ecdga gecad cdaegf dafc",
			"gedcf bcad abcfgd bd dcfbg bfgac fecgab bfd efgdcba faedbg | bd bd dacb cfgbd",
			"dcgfab fabed decbf gebcdf da efbag deca acfbegd dba fadcbe | ad abgef efdba debfc",
			"gadcfe gefdbc faced cfa geac afcgbd efbacgd ac cgdfe baedf | ca afc fcegd gedfca",
			"gefca ecdagb bg ebadf dbfg eagbf fbegdca cefbda beg fbedga | gb fabge gaebf ecbdag",
			"abd gdecbf da ecfbd afgbde ceda abdcf dfbcgae dbacef facbg | dafcb bda dgaebf gfabc",
			"afbegc cdgea aedfbg cgdb bgcaed egcdbfa cfeda cg dagbe gec | agedfb fabdge fegbda ecg",
			"ac acdbge dgbefa abc dbecfa cabgefd fegbc fbace eadfb fdac | ecbaf ca cgebadf afcd",
			"fdgcb cebf dcfbge cbdag gdbef acgdfe adgbecf fgc badfeg fc | ebcdgf cfg bgdac gfdeab",
			"gefc ecadgb efcabdg fca ecgad fc ceadgf bdfacg afebd cdaef | fca bgdeca efbad cf",
			"gcbfd ecdbf cfdgab gacfb dg gbda efcgba gfd egdcaf ebcadfg | gd gbacf facgb fcbed",
			"ecfa edfgcb bgdae abcdef ac debfc cad acbed febdgca bfcgad | edabc ca acd eadbc",
			"dbceaf cbagfe bdcgae bdc cdag debgf abcge bdgcaef dc cdebg | bdface gadc bgaec cebgaf",
			"ecf dfbae aecbf gabcf bfgdca cbegfd ec gcea cebfag cedabfg | afdeb efbca dbfea afbcdg",
			"bdagcfe gd debgca dgfc gbaef adbfce fcdbga gad dabcf fbagd | gdbeca dga cdbafg deabfc",
			"egabfc fdeb dcbae cfdea aeb be dabgc abcdef cagedf fegbacd | bdfe cedfag edbf gaefcd",
			"decfgba efgdc befdc dcabf ebc gbacdf acgbfe abedfc be deba | fdebc bec eabd fdagceb",
			"efbag gdae dgcbaef cdfbge gbfed gdcabf fedgba bag ag acebf | gbedf bafdcg cabdfg dgcfaeb",
			"dcfb agcdb aedfcg gfbdae fbgad dac cbeag dfbecag dc fdacgb | cagbe gadfbe dcfbga bgcad",
			"gefcb edcfg bega befdac bagfec cadbegf bce bcfag dcgafb eb | gbceaf bgcfa acefgb abfcg",
			"bgfce agbfc fga dabceg bdagfec afdcgb gcdba adfc fa baefgd | gfacbd fa agf gacbde",
			"gbfa bef bgeda cbeadg begdf gfcdaeb egcdf dfcaeb bdfage fb | fb dfbeca gfecd abgf",
			"dbfeac ecbfg ec dceg afbdcg gefbcd dbcfg geafb ecb bdegfac | fagdebc cadgbf gbdfc ec",
			"fgdbe beadfgc dfeab gbaf gf gdefca degcb aefbdg facbed gfd | fdcgae dgfbe fgd bfdae",
			"baecfd aeb cfae ae dgbaefc facdbg fbgead gbced bfdca bdeac | afce caef ecbda ea",
			"aedgb agf abefgcd fabgd afgebc faed dgfaeb bgfcd edbcag af | af adefgb adfe abdgec",
			"gdaecb gbcad gbcae cgfabd ebgfc ea bagcfed gae dbfage cdea | abdcg gadbc eacd adbegc",
			"fcdbe febacg gcfd egf ebgdf fecgdba cfdeba gf agbde fbcdge | gfe abged efg cdfg",
			"gb gdceafb bceaf cebgfa aedbcf fgb acgfb fcdag eagb cfbdge | efagbcd agdcbfe fbdegc abge",
			"ebfac cbeafg df dcbag acfdbe cfdbeg dfb gecfadb feda fdbac | gdcba gedfbc gedabcf cagbd",
			"cbfdg gbacf gdab bcdfeag dbf fgdec cfdabe dgbfac fgaceb bd | dcefba fgdce edfcg bafcg",
			"ec dacfeg aebc dfaegb eagdb gefcabd aedgcb dbcfg dec egbdc | fbadecg dfecag efgcdba dfbaeg",
			"gafbdc cbafde cfgebd gabcd cebag ad cad cafdebg bcgdf dgaf | fagd efcdbg acd bfdceg",
			"cde afebcdg bagcd ce fagedb dcfgea afbced efcg cgeda gfdae | acedbgf cabfed cefg dce",
			"cabg geabf gfbeca dfgea gfbeacd dgfceb gbe dcbeaf befac gb | agdfe dcfeab adefg eafgd",
			"debgacf edgab agedcb fdega ab cadfbg bad cbea bgecd dfbcge | adbge dabegc fagde abd",
			"bdefac afgbc abcef ce bdefga dfbea cef bgdfaec ecdb agfced | dcbe caebfd dgafec acbgf",
			"aecfbg gdaefc becgadf bdgfa cg efbcad decaf fcg acgfd edgc | cfbdgea cg cefdga cfg",
			"fd dgfcaeb fedg dcgbe bdgecf bfcde abcfdg dbf becaf bgeadc | cdgbea df egdcfab cdbfe",
			"aedcfb bdecg bcdefg cd fbadcge edc gdcf gbdef bfagde begac | befadc bfeadg fgdbe dc",
			"de cgadf gde gecfb egfbac bfde dcabeg ecbdgf efdacgb dcgfe | defb bgefc ged cbagfe",
			"egdfba cdafbge gfa dfgab cbdaf becgad bcgafe gfde bagde fg | bfcedga bacgde geabd cfbda",
			"bacgfd gec cbfeadg gdeaf afceg ec dagecb gacbf egafbc cefb | abgfcd fdabcge egc abcgfe",
			"edbgac fecbg adcgef fdbgc cbaeg feab bceafdg cgbfea fe efg | abfe fdbgc aebf cgadfe",
			"afbdcg dacbf cdbgf acdfe ebafcg ab beadfcg cfgbed cab dbag | becfgd abcdf bdcfaeg egbadcf",
			"dafceb gebfad cebg gbdfeac dfcga dgbac acedb gbd ebgcda bg | deabcg bcgade egcabd bdcafe",
			"gfa agcfe fbcae cedbaf cdfge ga gcfaedb afbedg cgba bgaefc | bagcfe ga fbace fceba",
			"eg cbaefgd dagfb adcbeg aeg cefabd bgec dgeafc abdce dbaeg | fcagde bdcage gabfd acdegb",
			"dfeagcb ebf eagfc gbefac cgdfea dbfag eb dcbafe gecb egfab | eb dgafb faecg eafdgc",
			"befdc gfbceda ecdgba bfdacg cde gecfb deaf bedafc ed cbdaf | becfd bdacgef eacgdfb gbdace",
			"agdb cdafegb cfabd cedbgf cdafeb fbgac bgc gb adcgbf faegc | cbgadf dabg fgeca cbgfa",
			"cfeabg cfeab fbge gcdae fgc aefdbgc gf gaefc dcgfba ebdfca | abefc cgf fbdaecg bafce",
			"cbfgda ebfdcg dgbfa bd fgdca abecgdf dgb cegfda bgfae cbda | gdcaf fdgab cbfaedg cgaefdb",
			"ef cbfgae abcdf adecfbg gacbe abecf agdbec ebgf dfceag cef | efc ecf dcbaf cbaeg",
			"dgcaf bgfd aebcd ebacgf cdafb fdabecg bdafgc adecfg bf fcb | afcdb fcadg fcadg adgefc",
			"afcgeb fdgc bcaefdg egacf ged gd faebdg cdaeg badce cfaged | cedba egcbaf dfbagce eagfc",
			"edgaf aegdfc dgcfba befdag cd cafdbeg cged facde eafcb fdc | gebfad gefda fcd agdfe",
			"debfa dcgbefa gdcbae cdfbae ab bdcegf cebfd bea cfba agedf | fgecdb bcaedg feagcdb fecbd",
			"abfecg fdegca dag cead dbfge gaedf da afcge cdgfba gbdcafe | gfedb bgfdac dag agdfe",
			"acedgb dagcebf abgcf acgbd fbgd afb cgadfb ebfcda fgcae bf | agcfe fgeac ebfdac bdcafe",
			"adecfg cbda ecabf egcbdaf fgaedb gcbef cfbaed bfa ab efdca | fbcae fcaeb dbac cfadeb",
			"aecgdf bgcea dbc facdgeb efdb db edgcb cdgfe cgdfba gcdfbe | fdgec bd gcdabf dbecg",
			"cbgdfe gbdfa dbe bfgdeca cgbdae cbefag becag ed aedc agebd | ecgab fgbdce aecd dace",
			"cfbed gfcbea abdefc gbfcd eabd dce de geadfc cbgdaef febac | ed egcafd cde edc",
			"fgcde fcbad fdegca ae fdcea dcegbf geaf eac badecg aecgbfd | egdcfa dbfac gafe ae",
			"bgadcf acgdf afgedc bdgeacf bfd cbaed fcgb agfdeb fb adbcf | fdbgca fdbcag acdegf cagfde",
			"gacbfde ebgdcf eabcfd fae fa bfead cabf cbefd gfcaed gaedb | fa fcdeb fecagdb bfac",
			"adecf cafbd bcdagf gbadc bf dbf gdebca edfagbc bfga dgcebf | fadbc dgcfbe dfb feacd",
			"dfgae fgcedba ge bcfade caedfg dfbga fdcea gaedcb gea gefc | cfeda fbagd gea eag",
			"dafceg dfba cdeabf da bedgafc adecb gecab fdecb febdgc cad | caebg cbfde abdf ecgab",
			"daebg cebg bcdae gb bdg cdfgba bfcagde dbaceg fdecba fdega | geadf dabge gebda ecabgd",
			"afedbg feb bf acfb dbeca bfegcad cdefg ecfdba dcfbe bedgca | facbedg cfegd cfba ecfdg",
			"bd abgcf cfgebd cfgdae abde dafceb fdb daecfgb fcdab cdaef | bd cbfda cabgf daeb",
			"ge cdfga edga caedfbg edbgfc dagfcb efcga cfagde feg faebc | gef eadg fcbagde efgbcd",
			"cabegd bec eafbd bagc dfcaegb dcgae cb ecdba bdgfec egfcda | cegdfb dgcae decfgb bcdea",
			"fcbgead af cdbga fabegc cbafd fead cefbd fbcdae gdebfc baf | cadbf cfdab efbdc baedcf",
			"edfbag bgc cfba ebgcaf cegdfab gfbae eadcgb fecgb fedgc cb | bdgeca cb cb dfgce",
			"bfadecg fad baedfc cabef gacfbe agecdf cgdfb aedb da acfdb | acebfgd aecfb efbca agbfdce",
			"defbca ebcdgaf fgbe gdcfa degcba decbgf fe defcg ecf bdegc | eadbgc fgbe gedfc dcegb",
			"bagecf bga gbde gb dacbf gefacdb edcagb cfgeda gdace cadgb | dgacb bg bdeg acdeg",
			"bdc dgbafe cabdeg bfgde befac defbc cd fcdg afgecbd gbfced | cbadeg cdfebg gaebcd gefdb",
			"afgce dc fabged gbafd fgcda gecdbf badc fdcebga cdf adcfgb | cfbdge dabc dcagbf dgfabe",
			"cdafeb egf afbeg dagbef cfbgead edag cgebdf gbcfa daefb eg | dabfe fbacg efg eagbf",
			"edbf afcbedg eacbgf fd efadc fagbdc abfcde daf becaf acdeg | cgfdba efabc adf fgcbda",
			"gcabe bcadeg dag dgafcbe agdcb faegcb dcbaf afebdg egcd dg | gdfeab cagbd bfceadg gda",
			"gafcd cbgafd gbafec fcgade bcfgeda cdea bedfg ea fae eagdf | acgdfe deacgf efgdb fdgac",
			"fecagb ae cgfbe gdafb fecdbg faegb gbcefda aefc aeg gecbda | gae ebgfc edgcfb adbegc",
			"fgced gcdfbae fec edcb ce bgdfe dfagbe acgdf cbaefg gbcdef | cdafg gaefcb becd cef",
			"begdc bacegfd cadfeg afbgc cbdagf egbcf feg bgacfe feba fe | fcbeg gbcaf egf fe",
			"bed eagdb fagdb efadbc aefbgc ecgd ed gbcdea cadgefb bgaec | bde de ed ecdg",
			"edbgfc ebfacg cdgef ga daegcf gfa bfacd aegd fgadc befgcad | cdbfa cfedabg dagcf gfbaecd",
			"gdacb bcdgfa egacdb facb cf cfd cgdefa gbfde egcafbd bcgdf | cfd aegcdb fdc cbfa",
			"fdca cagfb ac gcdbef bcfgade ebafg bdgcea abc cgfabd cgdbf | fgcbd febdagc bdgaec gbedfca",
			"ca dgcfb fgead gca edcgfa fagcd gecfba aedc dfeagb acegdbf | ac cga cade ca",
			"def gfbdc ebdgaf cbde gdcfe eabcdgf ecgfbd dbfgca gfcea de | dbec fecga dfbgc dcfeg",
			"dacfb bed gcbaefd fbcdag afdecb befc edfab agefd dabecg eb | dbe bdgfac abedcf cdbgfa",
			"abf cbadf cfagdb acgf fa fdbcg bfagde befcdg dcegabf cabde | agcf fa bdaec cfagbde",
			"edbcag aebcf afgb fcgbde beagcf bea cgefb ba edgfacb faced | aeb gbcdae ecbfa decbgf",
			"ad adgfcb afgdc ebafcg eadgfcb facgb ecgfd acdb gda gdeabf | fcegd cdgfa abefcg dcafg",
			"cfebd gfdec feadg gec gfabed cefdga cg gdca gebcfda aecbfg | cbfeag dcag gc cabdfeg",
			"eafdc fac af ebdcf bagcfd gfae eagdc bcfegad gebdca gcdefa | fcbgda dfebcga gdbace defcb",
			"dagcf aecdgbf bedfgc bafedg adcbg gdcbea cgb dabge cb ebac | fgdebc edfabg fdcag ebac",
			"bgdaf cbdgef bdcag ac abgedc afgceb cdegb acde ecgbfad cga | adec gbcda dbcge edac",
			"gbafe gabecf gaed edbgf ed abedgf bdgcf baefdc def cbgefad | febga agde fed gfdcb",
			"gefcba fgb gf eabdf gcabd edfacbg bcfadg dgfc dfgab gdeacb | fgadb gcdabf fg gfb",
			"gbadce cbdafeg ce egcdfa agdeb fbdac ecgb ebgdaf ced acedb | bcafd ecbg bcadf gedbacf",
			"ag fbcgead baefcg cfbgd feadc adge fgacd dcagfe dcafeb fga | gaed abdfce ag ecfbga",
			"efbgdc cbagf gcdabef fegcda gcdaf daeg gd deacf fgd acedfb | fadgc fgabc befdac dg",
			"ed gcefa dcefga fcdbgae dcgea efbdac acgfbe dbacg aed gfde | ed efcgba adgce de",
			"afceb adbfge beafg bga bedfgc ag bcadgef dgbfe gdaf geabdc | ebgcdf gfcbde ga edbfg",
			"gafcde agcfeb fdcge ebdfc gebfcda gef gf fgda eagcdb edcag | fabceg bgcfea fagd ebcdf",
			"ecadbf bedfg da dgfbcea bacfge ebfad bcaef deagbc adfc ead | dae fadc fdgceba debgf",
			"adb fbcae ad acfebd gefcbad cefgab bcdaf gdfcb ebagfd eacd | dbfca fdegbca abfecd cdbfg",
			"fbdcga ebfcd fbadc ca fabgdec bca bagfd bcgeda agfc bgefad | gafbcd cbedf afbdg afbdgc",
			"ecfdba cd facegb gbacde gcbed dce cegba bcdegfa befgd cgad | ced cd fcagebd dce",
			"ebdca fcabge dceagbf dgef fbedc ef feb cdfgb cdbfge cgafbd | bcade gfbcae cfgdeb efb",
			"eda egabfd egfacb da fdbegac gdecb adfc dacegf acdeg feacg | dcage cgbde faceg gcefa",
			"efcgda fgec dafebg agfcd aefgdcb gf ecadbg cgead fga adfbc | fegcda afdebg dbafc fag",
			"gbdaec gaecfd cbdag acfbd cfegabd fd fcd cgfadb fdbg fbcea | fd ecbagd fdgbeca ebdcfag",
			"dbgacf efgad abcf dcebagf ebgacd cfbdg dgcefb ab dfbga abd | dbgeacf cfdgba gbdecf gbfad",
			"abegfd cdafb bdfag ecagdf ag acegfdb ebdfg fag cdbfge bgea | fga dcegfa dgfeb fceagbd",
			"cd acbefg cdfge dfaceb gcaefbd cfd fcaeg egdbf dacg daegfc | egcfba cadg cd dcf",
			"gebdfca cdbefg afcg gecbf ag debacg dbaef fabge aeg gbcfea | cgbeda gfecdb ga cgebad",
			"cgdfeb dfacbg ecd cadfg cadbgfe aegdb fedgca acef cadeg ec | cgdae agbde dfcebg cgfdeba",
			"fg bdcagf ebacfg gbacd cgfd gfa dgbfa adcbeg edbfa dfabegc | aefbcg agbdc fdcg agcdfeb",
			"ba cbgefd fgace bae cadb edbfc abfedg caefdb caebf gaebcfd | bae cadgefb efdbc ba",
			"edcfb cdabe gacdeb dabfecg gefacd aedgbf dac bgac ca edbag | facedg dfeagc daegcb gdabce",
			"afbdce gfc fecagdb egdaf gfbaec bdcfe bcdegf efgdc cg bcgd | gbcfea dfgebc cdefb cg",
			"cgdaeb dfbacge cgdbe acb gcdfeb dfcga ba fcadbe gbae bcdag | fadbec cadgf ba ba",
			"cgdeb ebfcag gbdafc bgfde gecdba ec abgdc bce dgecafb cade | cdgafb dcea gdabfc bcagdf",
			"fadbec gecfbd deg ge afdgce bdfec agbcd ebfg cbedg fbedcag | bgef abcdef cgbde cbefgd",
			"aegdb befgcd gec ecgba aecd cfbag bdefga ce gacdeb dgecabf | eadc gbfced bgcae dgaeb",
			"cfabd fbedac cadfgb eb bce cdegf decgab eafcgdb cfbde afeb | fdbec fbacd be ebcadfg",
			"bdecga gdac cdbefg dc bcd baecg bdeac efabd fgceab dbacfeg | bedac faebd baced egbdca",
			"cg cafgdb bacfde gfaeb cfadb bagfc fcg ecbagfd fdcega dbcg | febag fcg bgcfad gadbcf",
			"fbde fgd df bagef gacdebf cefabg agdec gdafe gaebdf bcdgaf | efdb abfeg febd debafg",
			"aebcgd abcdfe bdfeg eaf dagecf cgaf bfacdeg fa gdeaf gedca | cedag gedfb agbecd afcg",
			"fedbcag cgfabd gba faegd eadbgc gceb bgeda bdcea gb aedbcf | beacd ecbg bdaec adgbec",
			"cfge gc gebfcda fbadc fdbcg degfb gebdac gdeafb dcg fcegdb | dcbgea debgf cdbaf bgcfd",
			"ceafb fegbdc bcfed ecd daefbg dfgecab cgfd dc aedgbc begdf | edbfg efcab cdfg bdgef",
			"ecag fegcba bfgac cdbfg efabc befdca afg gcbaedf abefgd ag | fedcba fgcbd ga acge",
			"badgf cgaeb ebdga afcbde ebdgaf fged acfgdbe de ebd fdbacg | gfdbea badefg dgeba beagc",
			"beagcd cga cafgedb ac febga fgcba cgfbd dgfcab gcefdb cfda | ac fcda agc ac",
			"dabefc abgfdce decfb gf gfed cbdgf bdegfc gfc gafcbe cagbd | cfg dcfeb bcefd edfcb",
			"gedab acd ceba agdcb bagdce fcgdb ca dgaebfc dfgbea fecagd | adc acd adegfc cda",
			"dg eafdg gcbeaf dcfaegb acgebd fadgbe dgbf daecf fgabe deg | gdfae bgfae fgbdea becdag",
			"decgfb bcgfdae agcbd afcdb fb ebaf fbd cedabf ecdfa ecfdga | dfeac fbae fb degabcf",
			"feabgdc gfdbe bgcad cf geacfd dafbcg cbgaed fabc cfg cdgbf | dbcag cf cebagdf gfdbe",
			"gdbef ecdgb efcd cdgba fabdeg fdcbeg faegcb ecg fdegcab ce | bgefcd gcdba bgdef egc",
			"gfebd abd cdfbga gcdea edcbfg abfe ba gebda dbeacgf eafdbg | beagd egdba ebdfg fdageb",
			"gafbcd bc bac fgeba fbdega agbec febc egadc bcfgaed beacfg | agbfe cfbeag gceda adegbf",
			"cdbg gfcead db gcbfde bdefc gefdc bdf fcdbega fecba dagbfe | dcabgef fegcd bgdc cgfbead",
			"bagcde efbcgd adgef fgaecdb bfeacg eabfg cebfg eab ab cfab | eafgb bacf ab eba",
			"ed dcfbeag dfcga cedb cbfae fgdabe dbcefa fbagce dfe dacef | afbgec de cadfg de",
			"dgc abfedg bdgea cbfde dfcgab ecag gbcdae cg gdfbcae gcbde | afbdeg dgabec begad gdc",
			"befcd dg bgacf fbcegad dgb gcad gbcdf fagdbe bfcage gfdcab | gdabef fdecb gcda agcd",
			"cb bgdcfe bgacefd fdgbe edcgb cegad cbagfd dbefag bdc efbc | cgade gbfaecd bcd cbdge",
			"adfcge afgd acfeb efcda da gdefc cegfbd bcegad fdbcage cad | defgbc aebgdc dgfa cadfeg",
			"gacdf bdg aegbdc bg debaf bfeg gadfbce gfbdea cfbeda gdfba | bg fgdca bg gb",
			"adbecg dfa fd gafbe gefda cefgda ecdag efdc abdfcg egbcdaf | fegba gfadec fadeg efcd",
			"ecfgad gbae ecdag ba agbdec bac fdabgc edabc cdgeabf cbfed | dcfeag cbegad cefadg gdebca",
			"fgbca cge gcefb gadebcf eabcfg gcadbf feag eg fecbd egdabc | ge afgdbc fgdbac egc",
			"eadg eadbc gfbcda badefc gdc fcgbe dcbge acfgbde dacgbe dg | cgd egbcd gcdfba agdbce",
			"ga befgac fcgedba fabced agfce edgfc gbea aebcf acg cagfdb | gecdf afebcd bgdfac cga"
		};
	}
}

//public static void Run()
//{
//	var input = Day8Input.Challenge;

//	var outputValues = input.SelectMany(line => line.Split(" | ")[1].Split(" "));
//	var solutionValues = outputValues.Where(value => value.Length == 2 || value.Length == 3 || value.Length == 4 || value.Length == 7);
//	var wires = new List<Digit>();
//	var total = 0;
//	var i = 1;

//	foreach (var line in input)
//	{
//		// Console.WriteLine(i++);
//		wires.Clear();

//		var parts = line.Split(" | ");
//		var digits = parts[0].Split(" ").ToList();
//		var output = parts[1].Split(" ");

//		// unique lengths
//		var one = digits.Single(d => d.Length == 2);
//		wires.Add(new Digit(1, one));
//		digits.Remove(one);

//		var four = digits.Single(d => d.Length == 4);
//		wires.Add(new Digit(4, four));
//		digits.Remove(four);

//		var seven = digits.Single(d => d.Length == 3);
//		wires.Add(new Digit(7, seven));
//		digits.Remove(seven);

//		var eight = digits.Single(d => d.Length == 7);
//		wires.Add(new Digit(8, eight));
//		digits.Remove(eight);


//		// from unique lengths
//		var nine = digits.Single(d => d.Length == 6 && d.ContainsAllOf(four));
//		wires.Add(new Digit(9, nine));
//		digits.Remove(nine);

//		var three = digits.Single(d => d.Length == 5 && d.ContainsAllOf(seven));
//		wires.Add(new Digit(3, three));
//		digits.Remove(three);

//		// from previous extrapolations
//		var five = digits.Single(d => d.Length == 5 && nine.ContainsAllOf(d));
//		wires.Add(new Digit(5, five));
//		digits.Remove(five);

//		var six = digits.Single(d => d.Length == 6 && d.ContainsAllOf(five));
//		wires.Add(new Digit(6, six));
//		digits.Remove(six);

//		var zero = digits.Single(d => d.Length == 6);
//		wires.Add(new Digit(0, zero));
//		digits.Remove(zero);

//		var two = digits.Single(d => d.Length == 5);
//		wires.Add(new Digit(2, two));
//		digits.Remove(two);


//		var lineDigitArray = output.Select(digit => wires.Single(wire => SortString(wire.SignalWires) == SortString(digit)).Value);
//		var lineValue = int.Parse(string.Join("", lineDigitArray));

//		total += lineValue;

//	}

//	Console.WriteLine(total);
//}