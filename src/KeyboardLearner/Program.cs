using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KeyboardLearner
{
    static class Program
    {
        public static string BaseDirectory = "C:\\Users\\aaron\\Desktop\\School\\Spring 2022\\Applied Software Project\\repo\\KeyboardLearner\\KeyboardLearner";
        
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Testing
            //Level test = Level.LoadLevel("C Major Scale");
            //List<Mapping> mappings = new List<Mapping>();
            //mappings.AddRange(new List<Mapping> {
            //    new Mapping('A', "c4"),
            //    new Mapping('S', "d4"),
            //    new Mapping('D', "e4"),
            //    new Mapping('F', "f4"),
            //    new Mapping('J', "g4"),
            //    new Mapping('K', "a4"),
            //    new Mapping('L', "b4"),
            //    new Mapping(';', "c5")});
            //test.AddMappings(mappings);

            //Level fur = new Level("Fur Elise", 2, 160, "888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888884",
            //    "e5,d#5,e5,d#5,e5,b4,d5,c5,a4,e3,a3,c4,e4,a4,b4,e3,g#3,e4,g#4,b4,c5,e3,a3,e4,e5,d#5,e5,d#5,e5,b4,d5,c5,a4,e3,a3,c4,e4,a4,b4,e3,g#3,e4,c5,b4,a4,e3,a4,e4,e5,d#5,e5,d#5,e5,b4,d5,c5,a4,e3,a3,c4,e4,a4,b4,e3,g#3,e4,g#4,b4,c5,e3,a3,e4,e5,d#5,e5,d#5,e5,b4,d5,c5,a4,e3,a3,c4,e4,a4,b4,e3,g#3,e4,c5,b4,a4,e3,a3,b4,c5,d5,e5,g3," +
            //    "c4,g4,f5,e5,d5,g3,b3,f4,e5,d5,c5,e3,a3,e4,d5,c5,b4,e3,e4,c5,b4,a4");
            //List<Mapping> mappings = new List<Mapping>();
            //mappings.AddRange(new List<Mapping> {
            //    new Mapping(';', "e5"),
            //    new Mapping('O', "d#5"),
            //    new Mapping('N', "b4"),
            //    new Mapping('L', "d5"),
            //    new Mapping('K', "c5"),
            //    new Mapping('H', "a4"),
            //    new Mapping('Z', "e3"),
            //    new Mapping('V', "a3"),
            //    new Mapping('D', "c4"),
            //    new Mapping('G', "e4"),
            //    new Mapping('S', "g#3"),
            //    new Mapping('I', "g#4"),
            //    new Mapping('A', "g3"),
            //    new Mapping('M', "g4"),
            //    new Mapping('P', "f5"),
            //    new Mapping('B', "f4"),
            //    new Mapping('F', "b3")
            //});
            //fur.AddMappings(mappings);

            //Level.SaveLevel(fur);


            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
