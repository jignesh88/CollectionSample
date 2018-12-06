using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace StringComparisionTest
{


    using System;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using System.Threading;

    public static class StringExtensions{

        /// <summary>
        /// Generates a Random Password
        /// respecting the given strength requirements.
        /// </summary>
        /// <param name="opts">A valid PasswordOptions object
        /// containing the password strength requirements.</param>
        /// <returns>A random password</returns>
        public static string GenerateRandomPassword(PasswordOptions opts =  null)
        {
            if (opts == null) opts = new PasswordOptions()
            {
                RequiredLength = 8,
                RequiredUniqueChars = 4,
                RequireDigit = true,
                RequireLowercase = true,
                RequireNonAlphanumeric = true,
                RequireUppercase = true
            };

            string[] randomChars = new[] {
        "ABCDEFGHJKLMNOPQRSTUVWXYZ",    // uppercase 
        "abcdefghijkmnopqrstuvwxyz",    // lowercase
        "0123456789",                   // digits
        "!@$?_-"                        // non-alphanumeric
    };
            Random rand = new Random(Environment.TickCount);
            List<char> chars = new List<char>();

            if (opts.RequireUppercase)
                chars.Insert(rand.Next(0, chars.Count),
                    randomChars[0][rand.Next(0, randomChars[0].Length)]);

            if (opts.RequireLowercase)
                chars.Insert(rand.Next(0, chars.Count),
                    randomChars[1][rand.Next(0, randomChars[1].Length)]);

            if (opts.RequireDigit)
                chars.Insert(rand.Next(0, chars.Count),
                    randomChars[2][rand.Next(0, randomChars[2].Length)]);

            if (opts.RequireNonAlphanumeric)
                chars.Insert(rand.Next(0, chars.Count),
                    randomChars[3][rand.Next(0, randomChars[3].Length)]);

            for (int i = chars.Count; i < opts.RequiredLength
                || chars.Distinct().Count() < opts.RequiredUniqueChars; i++)
            {
                string rcs = randomChars[rand.Next(0, randomChars.Length)];
                chars.Insert(rand.Next(0, chars.Count),
                    rcs[rand.Next(0, rcs.Length)]);
            }

            return new string(chars.ToArray());
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            DateTime end;
            DateTime start = DateTime.Now;

            Console.WriteLine("### Overall Start Time: " + start.ToLongTimeString());
            Console.WriteLine();

            //TestFastestWayToSeeIfAStringOccursInAString(5000, 1);
            //TestFastestWayToSeeIfAStringOccursInAString(25000, 1);
            //TestFastestWayToSeeIfAStringOccursInAString(100000, 1);
            //TestFastestWayToSeeIfAStringOccursInAString(1000000, 1);
            //TestFastestWayToSeeIfAStringOccursInAString(5000, 100);
            //TestFastestWayToSeeIfAStringOccursInAString(25000, 100);
            //TestFastestWayToSeeIfAStringOccursInAString(100000, 100);
            //TestFastestWayToSeeIfAStringOccursInAString(1000000, 100);
            //TestFastestWayToSeeIfAStringOccursInAString(5000, 1000);
            //TestFastestWayToSeeIfAStringOccursInAString(25000, 1000);
            //TestFastestWayToSeeIfAStringOccursInAString(100000, 1000);
            //TestFastestWayToSeeIfAStringOccursInAString(1000000, 1000);
            TestFastestWayToSeeIfAStringOccursInAString(50000, 1000);

            end = DateTime.Now;
            Console.WriteLine();
            Console.WriteLine("### Overall End Time: " + end.ToLongTimeString());
            Console.WriteLine("### Overall Run Time: " + (end - start));

            Console.WriteLine();
            Console.WriteLine("Hit Enter to Exit");
            Console.ReadLine();

        }

        //###############################################################

        //what is the fastest way to see if a string occurs in a string?

        static void TestFastestWayToSeeIfAStringOccursInAString(int NumberOfStringsToGenerate, int NumberOfSearchCharsToGenerate)
        {
            Console.WriteLine("######## " + System.Reflection.MethodBase.GetCurrentMethod().Name);
            Console.WriteLine("Number of Random Strings that will be generated: " + NumberOfStringsToGenerate.ToString("#,##0"));
            Console.WriteLine("Number of Search Strings that will be generated: " + NumberOfSearchCharsToGenerate.ToString("#,##0"));
            Console.WriteLine();

            object lockObject = new object();
            int total = 0;
            DateTime end = DateTime.Now;
            DateTime start = DateTime.Now;
            //the strings to search
            string[] ss = new string[NumberOfStringsToGenerate];
            //the chars/strings to look for. We use both because we're testing some string methods too.
            string[] sf = new string[NumberOfSearchCharsToGenerate];
            //the count of each substring finding
            int[] c = new int[sf.Length];

            //Generate the string arrays
            int z = 10;

            //yes I realize that most real world applications people are going to search strings
            //that are "human readable" and not random like I generate below. But this is a test
            //and I'm not about to type of millions of different strings for testing. :-)
            //strings to be searched. Completely random. Using generate password method to come up with all sorts of mixtures.
            Console.WriteLine("Generating strings to search.");
            for (int x = 0; x < ss.Length; x++)
            {
                ss[x] = StringExtensions.GenerateRandomPassword(new PasswordOptions { RequiredLength = z, RequireNonAlphanumeric = true });
                z += 1;
                if (z > 25)
                    z = 10;
            }

            //strings to search for
            Console.WriteLine("Generating strings to search for.");
            z = 2;
            for (int x = 0; x < sf.Length; x++)
            {
                sf[x] = StringExtensions.GenerateRandomPassword(new PasswordOptions { RequiredLength = z, RequireNonAlphanumeric = true });
                z += 1;
                if (z > 8)
                    z = 2;
            }
            Console.WriteLine("###########################################################");
            Console.WriteLine();

            //return (strToSearch.Length - strToSearch.Replace(strKeyToLookFor, String.Empty).Length) / strKeyToLookFor.Length;
            Console.WriteLine("Starting method: custom string method");
            start = DateTime.Now;
            for (int x = 0; x < ss.Length; x++)
            {
                for (int y = 0; y < sf.Length; y++)
                {
                    c[y] += ((ss[x].Length - ss[x].Replace(sf[y], String.Empty).Length) / sf[y].Length > 0 ? 1 : 0);
                }
            }
            end = DateTime.Now;
            Console.WriteLine("Finished at: " + end.ToLongTimeString());
            Console.WriteLine("Time: " + (end - start));
            total = 0;
            for (int x = 0; x < c.Length; x++)
            {
                total += c[x];
            }
            Console.WriteLine("Total finds: " + total + Environment.NewLine);
            Console.WriteLine();
            Console.WriteLine("###########################################################");
            Console.WriteLine();

            Array.Clear(c, 0, c.Length);
            Console.WriteLine("Starting method: Count split string on string ");
            start = DateTime.Now;
            for (int x = 0; x < ss.Length; x++)
            {
                for (int y = 0; y < sf.Length; y++)
                {
                    c[y] += (ss[x].Split(new string[] { sf[y] }, StringSplitOptions.None).Count() - 1 > 0 ? 1 : 0);
                }
            }
            end = DateTime.Now;
            Console.WriteLine("Finished at: " + end.ToLongTimeString());
            Console.WriteLine("Time: " + (end - start));
            total = 0;
            for (int x = 0; x < c.Length; x++)
            {
                total += c[x];
            }
            Console.WriteLine("Total finds: " + total + Environment.NewLine);
            Console.WriteLine();
            Console.WriteLine("###########################################################");
            Console.WriteLine();

            Array.Clear(c, 0, c.Length);
            Console.WriteLine("Starting method: string.contains ");
            start = DateTime.Now;
            for (int x = 0; x < ss.Length; x++)
            {
                for (int y = 0; y < sf.Length; y++)
                {
                    c[y] += (ss[x].Contains(sf[y]) == true ? 1 : 0);
                }
            }
            end = DateTime.Now;
            Console.WriteLine("Finished at: " + end.ToLongTimeString());
            Console.WriteLine("Time: " + (end - start));
            total = 0;
            for (int x = 0; x < c.Length; x++)
            {
                total += c[x];
            }
            Console.WriteLine("Total finds: " + total + Environment.NewLine);
            Console.WriteLine();
            Console.WriteLine("###########################################################");
            Console.WriteLine();

            Array.Clear(c, 0, c.Length);
            Console.WriteLine("Starting method: string.indexOf");
            start = DateTime.Now;
            for (int x = 0; x < ss.Length; x++)
            {
                for (int y = 0; y < sf.Length; y++)
                {
                    c[y] += (ss[x].IndexOf(sf[y]) >= 0 ? 1 : 0);
                }
            }
            end = DateTime.Now;
            Console.WriteLine("Finished at: " + end.ToLongTimeString());
            Console.WriteLine("Time: " + (end - start));
            total = 0;
            for (int x = 0; x < c.Length; x++)
            {
                total += c[x];
            }
            Console.WriteLine("Total finds: " + total + Environment.NewLine);
            Console.WriteLine();
            Console.WriteLine("###########################################################");
            Console.WriteLine();

            Array.Clear(c, 0, c.Length);
            Console.WriteLine("Starting method: linq contains usage ");
            start = DateTime.Now;
            for (int y = 0; y < sf.Length; y++)
            {
                c[y] += ss.Where(o => o.Contains(sf[y])).Count();
            }
            end = DateTime.Now;
            Console.WriteLine("Finished at: " + end.ToLongTimeString());
            Console.WriteLine("Time: " + (end - start));
            total = 0;
            for (int x = 0; x < c.Length; x++)
            {
                total += c[x];
            }
            Console.WriteLine("Total finds: " + total + Environment.NewLine);
            Console.WriteLine();
            Console.WriteLine("###########################################################");
            Console.WriteLine();

            //Array.Clear(c, 0, c.Length);
            //Console.WriteLine("Starting method: linq with IndexOf usage ");
            //start = DateTime.Now;
            //for (int y = 0; y < sf.Length; y++)
            //{
            //    c[y] += ss.Where(o => o.IndexOf(sf[y]) > -1).Count();
            //}
            //end = DateTime.Now;
            //Console.WriteLine("Finished at: " + end.ToLongTimeString());
            //Console.WriteLine("Time: " + (end - start));
            //total = 0;
            //for (int x = 0; x < c.Length; x++)
            //{
            //    total += c[x];
            //}
            //Console.WriteLine("Total finds: " + total + Environment.NewLine);
            //Console.WriteLine();
            //Console.WriteLine("###########################################################");
            //Console.WriteLine();

            Array.Clear(c, 0, c.Length);
            Console.WriteLine("Starting method: Regex IsMatch uncompiled ");
            start = DateTime.Now;
            for (int x = 0; x < ss.Length; x++)
            {
                for (int y = 0; y < sf.Length; y++)
                {
                    c[y] += Regex.IsMatch(ss[x], Regex.Escape(sf[y])) == true ? 1 : 0;
                }
            }
            end = DateTime.Now;
            Console.WriteLine("Finished at: " + end.ToLongTimeString());
            Console.WriteLine("Time: " + (end - start));
            total = 0;
            for (int x = 0; x < c.Length; x++)
            {
                total += c[x];
            }
            Console.WriteLine("Total finds: " + total + Environment.NewLine);
            Console.WriteLine();
            Console.WriteLine("###########################################################");
            Console.WriteLine();

            total = 0;
            Console.WriteLine("Starting method: Single AsParallel() Regex ");
            start = DateTime.Now;
            for (int x = 0; x < ss.Length; x++)
            {
                total += sf.AsParallel().Sum(s => Regex.IsMatch(ss[x], Regex.Escape(s)) ? 1 : 0);
            }
            end = DateTime.Now;
            Console.WriteLine("Finished at: " + end.ToLongTimeString());
            Console.WriteLine("Time: " + (end - start));
            Console.WriteLine("Total finds: " + total + Environment.NewLine);
            Console.WriteLine();
            Console.WriteLine("###########################################################");
            Console.WriteLine();

            total = 0;
            Console.WriteLine("Starting method: Parallel For Custom Counting");
            start = DateTime.Now;
            Parallel.For(0, ss.Length,
                () => 0,
                (x, loopState, subtotal) =>
                {
                    for (int y = 0; y < sf.Length; y++)
                    {
                        subtotal += ((ss[x].Length - ss[x].Replace(sf[y], String.Empty).Length) / sf[y].Length > 0 ? 1 : 0);
                    }
                    return subtotal;
                },
                (s) =>
                {
                    lock (lockObject)
                    {
                        total += s;
                    }
                }
            );
            end = DateTime.Now;
            Console.WriteLine("Finished at: " + end.ToLongTimeString());
            Console.WriteLine("Time: " + (end - start));
            Console.WriteLine("Total finds: " + total + Environment.NewLine);
            Console.WriteLine();
            Console.WriteLine("###########################################################");
            Console.WriteLine();

            total = 0;
            Console.WriteLine("Starting method: Parallel For Split string ");
            start = DateTime.Now;
            Parallel.For(0, ss.Length,
                () => 0,
                (x, loopState, subtotal) =>
                {
                    for (int y = 0; y < sf.Length; y++)
                    {
                        subtotal += (ss[x].Split(new string[] { sf[y] }, StringSplitOptions.None).Count() - 1 > 0 ? 1 : 0);
                    }
                    return subtotal;
                },
                (s) =>
                {
                    lock (lockObject)
                    {
                        total += s;
                    }
                }
            );
            end = DateTime.Now;
            Console.WriteLine("Finished at: " + end.ToLongTimeString());
            Console.WriteLine("Time: " + (end - start));
            Console.WriteLine("Total finds: " + total + Environment.NewLine);
            Console.WriteLine();
            Console.WriteLine("###########################################################");
            Console.WriteLine();

            total = 0;
            Console.WriteLine("Starting method: Parallel For String.Contains()" );
            start = DateTime.Now;
            Parallel.For(0, ss.Length,
                () => 0,
                (x, loopState, subtotal) =>
                {
                    for (int y = 0; y < sf.Length; y++)
                    {
                        subtotal += (ss[x].Contains(sf[y]) == true ? 1 : 0);
                    }
                    return subtotal;
                },
                (s) =>
                {
                    lock (lockObject)
                    {
                        total += s;
                    }
                }
            );
            end = DateTime.Now;
            Console.WriteLine("Finished at: " + end.ToLongTimeString());
            Console.WriteLine("Time: " + (end - start));
            Console.WriteLine("Total finds: " + total + Environment.NewLine);
            Console.WriteLine();
            Console.WriteLine("###########################################################");
            Console.WriteLine();

            total = 0;
            Console.WriteLine("Starting method: Parallel For String.IndexOf()");
            start = DateTime.Now;
            Parallel.For(0, ss.Length,
                () => 0,
                (x, loopState, subtotal) =>
                {
                    for (int y = 0; y < sf.Length; y++)
                    {
                        subtotal += (ss[x].IndexOf(sf[y]) >= 0 ? 1 : 0);
                    }
                    return subtotal;
                },
                (s) =>
                {
                    lock (lockObject)
                    {
                        total += s;
                    }
                }
            );
            end = DateTime.Now;
            Console.WriteLine("Finished at: " + end.ToLongTimeString());
            Console.WriteLine("Time: " + (end - start));
            Console.WriteLine("Total finds: " + total + Environment.NewLine);
            Console.WriteLine();
            Console.WriteLine("###########################################################");
            Console.WriteLine();

            total = 0;
            Console.WriteLine("Starting method: Parallel For Linq.Contains()");
            start = DateTime.Now;
            Parallel.For(0, sf.Length,
                () => 0,
                (x, loopState, subtotal) =>
                {
                    subtotal += ss.Where(o => o.Contains(sf[x])).Count();
                    return subtotal;
                },
                (s) =>
                {
                    lock (lockObject)
                    {
                        total += s;
                    }
                }
            );
            end = DateTime.Now;
            Console.WriteLine("Finished at: " + end.ToLongTimeString());
            Console.WriteLine("Time: " + (end - start));
            Console.WriteLine("Total finds: " + total + Environment.NewLine);
            Console.WriteLine();
            Console.WriteLine("###########################################################");
            Console.WriteLine();

            total = 0;
            Console.WriteLine("Starting method: Parallel For Linq.IndexOf()");
            start = DateTime.Now;
            Parallel.For(0, sf.Length,
                () => 0,
                (x, loopState, subtotal) =>
                {
                    subtotal += ss.Where(o => o.IndexOf(sf[x]) > -1).Count();
                    return subtotal;
                },
                (s) =>
                {
                    lock (lockObject)
                    {
                        total += s;
                    }
                }
            );
            end = DateTime.Now;
            Console.WriteLine("Finished at: " + end.ToLongTimeString());
            Console.WriteLine("Time: " + (end - start));
            Console.WriteLine("Total finds: " + total + Environment.NewLine);
            Console.WriteLine();
            Console.WriteLine("###########################################################");
            Console.WriteLine();

            total = 0;
            Console.WriteLine("Starting method: Parallel For Regex.IsMatch()");
            start = DateTime.Now;
            Parallel.For(0, ss.Length,
                () => 0,
                (x, loopState, subtotal) =>
                {
                    for (int y = 0; y < sf.Length; y++)
                    {
                        subtotal += Regex.IsMatch(ss[x], Regex.Escape(sf[y])) == true ? 1 : 0;
                    }
                    return subtotal;
                },
                (s) =>
                {
                    lock (lockObject)
                    {
                        total += s;
                    }
                }
            );
            end = DateTime.Now;
            Console.WriteLine("Finished at: " + end.ToLongTimeString());
            Console.WriteLine("Time: " + (end - start));
            Console.WriteLine("Total finds: " + total + Environment.NewLine);
            Console.WriteLine();
            Console.WriteLine("###########################################################");
            Console.WriteLine();

            total = 0;
            Console.WriteLine("Starting method: Parallel For AsParallel() Regex ");
            start = DateTime.Now;
            Parallel.For(0, ss.Length,
                () => 0,
                (x, loopState, subtotal) =>
                {
                    subtotal += sf.AsParallel().Sum(s => Regex.IsMatch(ss[x], Regex.Escape(s)) ? 1 : 0);
                    return subtotal;
                },
                (s) =>
                {
                    lock (lockObject)
                    {
                        total += s;
                    }
                }
            );
            end = DateTime.Now;
            Console.WriteLine("Finished at: " + end.ToLongTimeString());
            Console.WriteLine("Time: " + (end - start));
            Console.WriteLine("Total finds: " + total + Environment.NewLine);
            Console.WriteLine();
            Console.WriteLine("###########################################################");
            Console.WriteLine();

            Array.Clear(ss, 0, ss.Length);
            ss = null;
            Array.Clear(sf, 0, sf.Length);
            sf = null;
            Array.Clear(c, 0, c.Length);
            c = null;
            GC.Collect();

        }
    }


    //public static class CollectionExtension
    //{

    //    public static bool Contains<Keyword>(this IEnumerable<TransactionGroup> source, 
    //                                         Keyword value)
    //    {
    //        foreach (var i in source)
    //        {
    //            if (i.Description.Contains(value.ToString()))
    //                return true;
    //        }
    //        return false;
    //    }
    //}

  
    //public class Keyword {
    //    public string KeywordName { get; set; }
    //    public int KeywordTypeId { get; set; }
    //    public string Description { get; set; }
    //    public override string ToString()
    //    {
    //        return this.Description;
    //    }
    //}

    //public class TransactionGroup : IEqualityComparer<Keyword> {
    //    public string Description { get; set; }

    //    public bool Equals(Keyword x, Keyword y)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public int GetHashCode(Keyword obj)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}

    //class Program
    //{
    //    static void Main(string[] args)
    //    {


    //        StringComparision stringComparison = new StringComparision();

    //        System.Diagnostics.Stopwatch sp = new System.Diagnostics.Stopwatch();
    //        List<TransactionGroup> list = new List<TransactionGroup> {
    //            new TransactionGroup{ Description = "jignesh patel" },
    //            new TransactionGroup{ Description = "mahek patel"}
    //        };

    //        List<Keyword> keywords = new List<Keyword> ();

    //        for (int i = 0; i < 600000; i++)
    //        {
    //            list.Add(new TransactionGroup { Description = $" JIGNESH{i} patel{i} Make{i}" });
    //        }

    //        for (int i = 0; i < 20000; i++)
    //        {
    //            keywords.Add(new Keyword { Description = $"JIGNESH{i}" });
    //        }
    //        sp.Start();



    //        foreach (var item in keywords)
    //        {
    //            if (list.Any(l=> item.Description.Contains(item)) ){
    //                Console.WriteLine($"I FOUND { item.Description }");
    //            }
    //        }
    //        sp.Stop();
    //        Console.WriteLine($" Time  { sp.Elapsed.Seconds } ");


    //        Console.ReadKey();

    //        stringComparison.ListOfStrings = new List<string> {
    //            "purchase at north sydney",
    //            "fuel at parramatta",
    //            "buy food from baulkham hills"
    //        };

    //        stringComparison.ListOfItemToFind = new List<string>
    //        {
    //            "north sydney",
    //            "baulkham hills"
    //        };


    //        var output = stringComparison.ListOfStrings.Where(c => c.Contains("sydney"));

    //        if(output.Count() > 0) {
    //            Console.WriteLine("We found it");
    //        }

    //        //stringComparison.StringContains();

    //        //stringComparison.StringBinarySearch();
    //    }
    //}


    //public class StringComparision {

    //    public List<string> ListOfStrings { get; set; }

    //    public List<string> ListOfItemToFind { get; set; }

    //    public HashSet<string> ListOfThings { get; set; }

    //    public void StringContains()
    //    {
    //        foreach (var item in ListOfStrings)
    //        {
    //            if(ListOfStrings.Contains(item))
    //            {
    //                Console.WriteLine($" Contains :  I contains  {item }");
    //            }            
    //        }
    //    }

    //    public void StringBinarySearch()
    //    {
    //        foreach (var item in ListOfItemToFind)
    //        {
    //            ListOfStrings.Sort();
    //            var index = ListOfStrings.BinarySearch(item);
    //            if(index > 0){
    //                Console.WriteLine($" Binary Search : I contains {item} at index { index }");
    //            } else {
    //                Console.WriteLine($" Binary Search : unable to find");                    
    //            }
    //        }
    //    }


    //}
}
