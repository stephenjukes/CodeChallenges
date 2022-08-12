using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;


// Five Ways to Crack a Vigenère Cipher
// https://www.cipherchallenge.org
namespace NbsCodeChallenges
{
    public class nbsChq_Codebreaker
    {
        private static string _alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        public static void Run()
        {
            var caesarShiftCipher = "MHILY LZA ZBHL XBPZXBL MVYABUHL HWWPBZ JSHBKPBZ JHLJBZ KPJABT HYJHUBT LZA ULBAYVU";
            var simpleMonoalphabeticSubstitutionCipher = "BT JPX RMLX PCUV AMLX ICVJP IBTWXVR CI M LMT’R PMTN, MTN YVCJX CDXV MWMBTRJ JPX AMTNGXRJBAH UQCT JPX QGMRJXV CI JPX YMGG CI JPX HBTW’R QMGMAX; MTN JPX HBTW RMY JPX QMVJ CI JPX PMTN JPMJ YVCJX. JPXT JPX HBTW’R ACUTJXTMTAX YMR APMTWXN, MTN PBR JPCUWPJR JVCUFGXN PBL, RC JPMJ JPX SCBTJR CI PBR GCBTR YXVX GCCRXN, MTN PBR HTXXR RLCJX CTX MWMBTRJ MTCJPXV. JPX HBTW AVBXN MGCUN JC FVBTW BT JPX MRJVCGCWXVR, JPX APMGNXMTR, MTN JPX RCCJPRMEXVR. MTN JPX HBTW RQMHX, MTN RMBN JC JPX YBRX LXT CI FMFEGCT, YPCRCXDXV RPMGG VXMN JPBR YVBJBTW, MTN RPCY LX JPX BTJXVQVXJMJBCT JPXVXCI, RPMGG FX AGCJPXN YBJP RAMVGXJ, MTN PMDX M APMBT CI WCGN MFCUJ PBR TXAH, MTN RPMGG FX JPX JPBVN VUGXV BT JPX HBTWNCL. JPX RXACTN ACNXYCVN BR CJPXGGC.";
            var vigenereChipher = "K Q O W E F V J P U J U U N U K G L M E K J I N M W U X F Q M K J B G W R L F N F G H U D W U U M B S V L P S N C M U E K Q C T E S W R E E K O Y S S I W C T U A X Y O T A P X P L W P N T C G O J B G F Q H T D W X I Z A Y G F F N S X C S E Y N C T S S P N T U J N Y T G G W Z G R W U U N E J U U Q E A P Y M E K Q H U I D U X F P G U Y T S M T F F S H N U O C Z G M R U W E Y T R G K M E E D C T V R E C F B D J Q C U S W V B P N L G O Y L S K M T E F V J J T W W M F M W P N M E M T M H R S P X F S S K F F S T N U O C Z G M D O E O Y E E K C P J R G P M U R S K H F R S E I U E V G O Y C W X I Z A Y G O S A A N Y D O E O Y J L W U N H A M E B F E L X Y V L W N O J N S I O F R W U C C E S W K V I D G M U C G O C R U W G N M A A F F V N S I U D E K Q H C E U C P F C M P V S U D G A V E M N Y M A M V L F M A O Y F N T Q C U A F V F J N X K L N E I W C W O D C C U L W R I F T W G M U S W O V M A T N Y B U H T C O C W F Y T N M G Y T Q M K B B N L G F B T W O J F T W G N T E J K N E E D C L D H W T V B U V G F B I J G Y Y I D G M V R D G M P L S W G J L A G O E E K J O F E K N Y N O L R I V R W V U H E I W U U R W G M U T J C D B N K G M B I D G M E E Y G U O T D G G Q E U J Y O T V G G B R U J Y S";
            var monoalphabeticCipherWithHomophones = "IXDVMUFXLFEEFXSOQXYQVXSQTUIXWF*FMXYQVFJ*FXEFQUQXJFPTUFXMX*ISSFLQTUQXMXRPQEUMXUMTUIXYFSSFI*MXKFJF*FMXLQXTIEUVFXEQTEFXSOQXLQ*XVFWMTQTUQXTITXKIJ*FMUQXTQJMVX*QEYQVFQTHMXLFVQUVIXM*XEI*XLQ*XWITLIXEQTHGXJQTUQXSITEFLQVGUQX*GXKIEUVGXEQWQTHGXDGUFXTITXDIEUQXGXKFKQVXSIWQXAVPUFXWGXYQVXEQJPFVXKFVUPUQXQXSGTIESQTHGX*FXWFQFXSIWYGJTFXDQSFIXEFXGJPUFXSITXRPQEUGXIVGHFITXYFSSFI*CXC*XSCWWFTIXSOQXCXYQTCXYIESFCX*FXCKVQFXVFUQTPUFXQXKI*UCXTIEUVCXYIYYCXTQ*XWCUUFTIXLQFXVQWFXDCSQWWIXC*FXC*XDI**QXKI*IXEQWYVQXCSRPFEUCTLIXLC*X*CUIXWCTSFTIXUPUUQX*QXEUQ**QXJFCXLQX*C*UVIXYI*IXKQLQCX*CXTIUUQXQX*XTIEUVIXUCTUIXACEEIXSOQXTITXEPVJQCXDPIVXLQ*XWCVFTXEPI*IXSFTRPQXKI*UQXVCSSQEIXQXUCTUIXSCEEIX*IX*PWQXQVZXLFXEIUUIXLZX*ZX*PTZXYIFXSOQXTUVZUFXQVZKZWXTQX*Z*UIXYZEEIRPZTLIXTZYYZVKQXPTZXWITUZJTZXAVPTZXYQVX*ZXLFEUZTHZXQXYZVKQWFXZ*UZXUZTUIXRPZTUIXKQLPUZXTITXZKQZXZ*SPTZXTIFXSFXZ**QJVNWWIXQXUIEUIXUIVTIXFTXYFNTUIXSOQXLQX*NXTIKNXUQVVNXPTXUPVAIXTNSRPQXQXYQVSIEEQXLQ*X*QJTIXF*XYVFWIXSNTUIXUVQXKI*UQXF*XDQXJFVBVXSITXUPUUQX*BSRPQXBX*BXRPBVUBX*QKBVX*BXYIYYBXFTXEPEIXQX*BXYVIVBXFVQXFTXJFPXSIWB*UVPFXYFBSRPQFTDFTXSOQX*XWBVXDPXEIYVBXTIFXVFSOFPEIXX*BXYBVI*BXFTXSILFSQXQXQRPBUIV";

            // CHECK LANGUAGE !!!
            // var characterFrequencyByLanguage = GetCharacterFrequencyByLanguage("./../../../nbsChq_Codebreaker/characterFrequencyByLanguage.csv");
            var nGramLength = 2;

            var nGramFrequencies = GetNGramFrequencies(nGramLength, "The quick brown fox jumps over the lazy dog".ToUpper().Replace(" ", string.Empty));
            var fitness = GetFitness(nGramLength, vigenereChipher.Replace(" ", string.Empty), nGramFrequencies);

        }

        private static Dictionary<string, decimal> GetNGramFrequencies(int n, string text)
        {
            var nGramFrequencies = new Dictionary<string, decimal>();
            var testScope = text.Length + 1 - n;

            for (var i = 0; i < testScope; i++)
            {
                var nGram = text.Substring(i, n);
                if (nGramFrequencies.ContainsKey(nGram))
                {
                    nGramFrequencies[nGram] += 1 / (decimal)testScope;  // get average afterwards if not performant 
                }
                else
                {
                    nGramFrequencies[nGram] = 1 / (decimal)testScope;   // get average afterwards if not performant 
                }
            }

            return nGramFrequencies;
        }

        private static decimal GetFitness(int n, string text, Dictionary<string, decimal> nGramFrequencies)
        {
            // this repeats much of GetNgramFrequencies. Refactor.
            var testScope = text.Length + 1 - n;
            var result = 0m;

            for(var i = 0; i < testScope; i++)
            {
                var nGram = text.Substring(i, n);

                result = nGramFrequencies.ContainsKey(nGram)
                    ? result + (decimal)Math.Log((double)nGramFrequencies[nGram])
                    : result + 0;
            }

            return result / testScope;
        }



























        //private static IEnumerable<decimal> GetNGramFrequencies(int n, string text)
        //{
        //    var nGramCombinations = (int)Math.Pow(26, n);
        //    var nGramFrequencies = new int[nGramCombinations];
        //    var testScope = text.Length + 1 - n;

        //    for (var i = 0; i < testScope; i++)
        //    {
        //        var nGramFrequenciesIndex = GetNgramFrequenciesIndex(text.Substring(i, n));
        //        nGramFrequencies[nGramFrequenciesIndex] += 1;
        //    }

        //    var averageFrequencies = nGramFrequencies.Select(f => f / (decimal)testScope);

        //    return averageFrequencies;
        //}

        //private static int GetNgramFrequenciesIndex(string nGram)
        //{
        //    return nGram.Length > 1
        //        ? _alphabet.IndexOf(nGram[0]) * (int)Math.Pow(26, nGram.Length - 1) + GetNgramFrequenciesIndex(nGram.Substring(1))
        //        : _alphabet.IndexOf(nGram);
        //}

        // var characterFrequency = GetFrequency(caesarShiftCipher.ToCharArray().Select(ch => ch.ToString()));
        // var wordFrequency = GetFrequency(caesarShiftCipher.Split(" "));


        //    private static List<CharacterFrequencyByLanguage> GetCharacterFrequencyByLanguage(string path)
        //    {
        //        var characterSet = "abcdefghijklmnopqrstuvwxyz"; // this really needs to be taken from the data

        //        List<CharacterFrequencyByLanguage> data = new List<CharacterFrequencyByLanguage>(); 
        //        using (var reader = new StreamReader(path))
        //        {
        //            while (!reader.EndOfStream)
        //            {
        //                var rowData = reader.ReadLine().Split(',');

        //                data.Add(new CharacterFrequencyByLanguage(characterSet, rowData[0], rowData.Skip(1)));
        //            }
        //        }

        //        return data;
        //    }


        //    private static Dictionary<string, int> GetFrequency(IEnumerable<string> textArray)
        //    {
        //        var frequencies = textArray.Aggregate(new Dictionary<string, int>(), (dictionary, character) =>
        //            {
        //                if (dictionary.ContainsKey(character))
        //                {
        //                    dictionary[character]++;
        //                }
        //                else
        //                {
        //                    dictionary[character] = 1;
        //                }

        //                return dictionary;
        //            });

        //        frequencies.Remove(" ");    // spaces

        //        return frequencies;
        //    }

        //    //private static Dictionary<string, double> GetLanguageProbabilities(Dictionary<string, int> characterFrequencies)
        //    //{

        //    //}
        //}

        //public class CharacterFrequencyByLanguage
        //{
        //    private string _characterSet;

        //    public CharacterFrequencyByLanguage(string characterSet, string language, IEnumerable<string> characterFrequencies)
        //    {
        //        _characterSet = characterSet;
        //        Language = language;
        //        CharacterFrequencies = ResolveCharacterFrequencies(characterFrequencies);
        //    }

        //    public string Language { get; set; }
        //    public IEnumerable<LanguageCharacterFrequency> CharacterFrequencies { get; set; }

        //    private IEnumerable<LanguageCharacterFrequency> ResolveCharacterFrequencies(IEnumerable<string> characterFrequencies)
        //    {
        //        return characterFrequencies.Select((frequency, i) =>
        //            new LanguageCharacterFrequency(_characterSet[i].ToString(), Double.Parse(frequency)));
        //    }
        //}

        //public class LanguageCharacterFrequency
        //{
        //    public LanguageCharacterFrequency(string character, double frequency)
        //    {
        //        Character = character;
        //        Frequency = frequency;
        //    }

        //    public string Character { get; set; }
        //    public double Frequency { get; set; }
        //}

        //public class CodeCharacterFrequency
        //{ 
        //    public string Character { get; set; }   // let's make this a string for now in case this needs to become a larger entity
        //    public int Frequency { get; set; }
        //}    //    private static List<CharacterFrequencyByLanguage> GetCharacterFrequencyByLanguage(string path)
        //    {
        //        var characterSet = "abcdefghijklmnopqrstuvwxyz"; // this really needs to be taken from the data

        //        List<CharacterFrequencyByLanguage> data = new List<CharacterFrequencyByLanguage>(); 
        //        using (var reader = new StreamReader(path))
        //        {
        //            while (!reader.EndOfStream)
        //            {
        //                var rowData = reader.ReadLine().Split(',');

        //                data.Add(new CharacterFrequencyByLanguage(characterSet, rowData[0], rowData.Skip(1)));
        //            }
        //        }

        //        return data;
        //    }


        //    private static Dictionary<string, int> GetFrequency(IEnumerable<string> textArray)
        //    {
        //        var frequencies = textArray.Aggregate(new Dictionary<string, int>(), (dictionary, character) =>
        //            {
        //                if (dictionary.ContainsKey(character))
        //                {
        //                    dictionary[character]++;
        //                }
        //                else
        //                {
        //                    dictionary[character] = 1;
        //                }

        //                return dictionary;
        //            });

        //        frequencies.Remove(" ");    // spaces

        //        return frequencies;
        //    }

        //    //private static Dictionary<string, double> GetLanguageProbabilities(Dictionary<string, int> characterFrequencies)
        //    //{

        //    //}
        //}

        //public class CharacterFrequencyByLanguage
        //{
        //    private string _characterSet;

        //    public CharacterFrequencyByLanguage(string characterSet, string language, IEnumerable<string> characterFrequencies)
        //    {
        //        _characterSet = characterSet;
        //        Language = language;
        //        CharacterFrequencies = ResolveCharacterFrequencies(characterFrequencies);
        //    }

        //    public string Language { get; set; }
        //    public IEnumerable<LanguageCharacterFrequency> CharacterFrequencies { get; set; }

        //    private IEnumerable<LanguageCharacterFrequency> ResolveCharacterFrequencies(IEnumerable<string> characterFrequencies)
        //    {
        //        return characterFrequencies.Select((frequency, i) =>
        //            new LanguageCharacterFrequency(_characterSet[i].ToString(), Double.Parse(frequency)));
        //    }
        //}

        //public class LanguageCharacterFrequency
        //{
        //    public LanguageCharacterFrequency(string character, double frequency)
        //    {
        //        Character = character;
        //        Frequency = frequency;
        //    }

        //    public string Character { get; set; }
        //    public double Frequency { get; set; }
        //}

        //public class CodeCharacterFrequency
        //{ 
        //    public string Character { get; set; }   // let's make this a string for now in case this needs to become a larger entity
        //    public int Frequency { get; set; }
        //}
    }
}
