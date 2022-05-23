using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using testforskoleprosjekt;

namespace SystemLibrary
{
    public class Program
    {
        Random random = new Random();
        public int[] todaysPrices = new int[24];
        public int[] sortedPrices = new int[24];
        public int currentHour = 0;
        List<threshold> thresholdlist = new List<threshold>();
        List<HourOff> hourofflist = new List<HourOff>();
        public int getHour = 0; //wont work

        public static void Main(string[] args){
            Program program = new Program();

            string answer = Convert.ToString(Console.ReadLine());
            if (answer != "ass")
            {
                program.pricesToday();
                Console.WriteLine("======");
                program.findTodaysTops();
                Console.WriteLine("======");
                program.insertThreshold();
                Console.WriteLine("======");
                program.turnOff();                
                Console.WriteLine("======");
            }
        }

        int makeID()
        {
            int ID = 0;
            string writtenID = "";
            for (int i = 0; i < 6; i++)
            {
                writtenID = writtenID + Convert.ToString(random.Next(0, 9));
            }

            ID = Convert.ToInt32(writtenID);
            return ID;
        }

        void insertThreshold()
        {
            Console.WriteLine("Do you want to add a new item? (n for no)");
            Console.WriteLine("==============================");
            string answer = Convert.ToString(Console.ReadLine());                            

            if (answer != "n")
            {
                threshold threshold = new threshold(); // Kaller på threshold objekt.
                int genID = makeID(); // Skaffer ID.
                Console.WriteLine($"At what threshold (percent over average price) do you want your system to turn off? ID = {genID}"); // Spørsmål.
                Console.WriteLine("==================================================================================================");
                float chosenThreshold = Convert.ToInt32(Console.ReadLine()); // Spør om hva bruker ønsker som threshold.
                chosenThreshold = chosenThreshold / 100; // Tilpass prosenten for senere matte.
                threshold.id = genID; // Legg id til listen.
                threshold.pricethreshold = chosenThreshold; // legg valgte thresholden til listen.
                thresholdlist.Add(threshold); // Legg til threshold inn i threshold listen.
            }

            Console.WriteLine("Another one?"); // Spør om bruker vil legge til flere thresholds.
            Console.WriteLine("==============================");
            answer = Convert.ToString(Console.ReadLine()); // Spør om svar fra bruker.

            if (answer != "n") // Så lenge svaret til brukeren ikke er n,
            {
                insertThreshold(); // Kaller seg selv. rekusjon.
            }
            else
            {
                int a = thresholdlist.Count;
                for (int i = 0; i < a; i++) // så lenge i er mindre enn mengden av mengden thresholds.
                {
                    string id = Convert.ToString(thresholdlist.ElementAt(i).id); // Hent id.                                                                                                                                    
                    string thresholdanswer = Convert.ToString(thresholdlist.ElementAt(i).pricethreshold * 100 + "%"); // Hent threshold fra bruker.
                    string returned = id + " = " + thresholdanswer; // Lager en string med informasjonen programmet skal vise deg etter valget av threshold.
                    Console.WriteLine(returned); // Viser valget ditt av threshold.
                }   
            }
        }

        int GenerateRandomPrice() // Prisgenerator
        {
            int result = 0; // Deklarerer variabelen result

            int NewPrice = random.Next(0, 1000); // Her skaffer vi et tilfeldig tall mellom 0 og 1000. Dette blir prisen.
            int AddorSubstract = random.Next(0, 4); // Her flipper vi en mynt for å finne ut om prisen stiger eller synker.

            if (AddorSubstract <= 1) // Her legges den tilfeldige prisen til.
            {
                result = result + NewPrice;
            }
            else if (AddorSubstract >= 2) // Her trekkes prisen fra.
            {
                result = result - NewPrice;
            }

            return result; // Returnerer resultatet.
        }

        public int[] SortArray(int[] array, int leftIndex, int rightIndex) // Dette er en quick-sort metode, noe som jeg har lånt fra en veldig bra mann på nett som vi ofte gjør som kodere.
        {                                                                 // Så jeg aner ikke hva dette gjør.
            var i = leftIndex;
            var j = rightIndex;
            var pivot = array[leftIndex];

            while (i <= j)
            {
                while (array[i] < pivot)
                {
                    i++;
                }

                while (array[j] > pivot)
                {
                    j--;
                }

                if (i <= j)
                {
                    int temp = array[i];
                    array[i] = array[j];
                    array[j] = temp;
                    i++;
                    j--;
                }
            }

            if (leftIndex < j)
                SortArray(array, leftIndex, j);

            if (i < rightIndex)
                SortArray(array, i, rightIndex);

            return array;
        }

        void pricesToday()
        {
            int currentPrice = todaysPrices[currentHour]; // Kopierer timens pris over i nåprisen.
            int i = 0;

            if (currentPrice < 0)
            {
                currentPrice = currentPrice + 1000; // Hvis den tidligere prisen er under 0 så legges 1000 på.
            } 

            while (i < todaysPrices.Length) // Så lenge det er priser å regne ut.
            {
                int getPrice = GenerateRandomPrice(); // Her kaller jeg på prisgeneratoren for å gi oss en tilfeldig pris.

                if (getPrice < 0)
                {
                    getPrice = getPrice + 1000; // Her legges 1000 til getPrice så lenge getPrice er mindre enn 0.
                } 

                int addPrice = getPrice + currentPrice; // Her legger jeg sammen getPrice og currentPrice som er prisen fra døgnet før.
                todaysPrices[i] = addPrice; // Her legger jeg prisen for denne runden inn i en array for dagens priser.
                Console.WriteLine(todaysPrices[i]); // Her viser jeg dages pris.
                i++;
            }
        }

        void findTodaysTops()
        {
            for (int i = 0; i < todaysPrices.Length; i++) // Kjører for pris per time.
            {
                sortedPrices[i] = todaysPrices[i]; // Her kopierer jeg dagens pris over i en annen array for å ikke tulle til den orginale arrayen.
            }

            SortArray(sortedPrices, 0, sortedPrices.Length - 1); // Her bruker jeg quick-sort for å sortere dagens priser.

            int count = sortedPrices.Length - 1; // Her lagrer jeg antall objekter som finnes i sortedPrices.
            int top1 = sortedPrices[count]; // Her leter jeg fram de 3 toppene fra listen.
            int top2 = sortedPrices[count - 1];
            int top3 = sortedPrices[count - 2];

            int j = 0; // Her deklarerer jeg en variabel for å telle antall kjøringer while-løkka under har gjennomgått.
            while (j < sortedPrices.Length)
            {
                Console.WriteLine(sortedPrices[j]); // Her viser jeg den soterte prislisten.
                j++;
            }
            Console.WriteLine("=====");
            Console.WriteLine(top1); // Her viser jeg topp 1, 2 og 3
            Console.WriteLine(top2);
            Console.WriteLine(top3);
        }

        public void turnOff()
        {
            List<HourOff> results = new List<HourOff>(); // Her deklarerer jeg en ny HourOff liste. Denne er navnet results.

            for (int i = 0; i < thresholdlist.Count; i++) // Så lenge det er objekter
            {
                for (int j = 0; j < todaysPrices.Length; j++) // og så lenge det er timer å beregne status for, så vil det regnes ut status.
                {
                    HourOff houroff = new HourOff(); // Her deklarerer jeg et nytt HourOff objekt.

                    float firstTotal = todaysPrices[j] * thresholdlist[i].pricethreshold;
                    float amount = todaysPrices.Length;
                    float result = 0;

                    for (int k = 0; k < amount; k++) // finner totalprisen for 24 timer med strøm.
                    {
                        float current = todaysPrices[k];
                        float last = result;
                        result = last + current;
                    }
                    float average = result / amount; // deler totalprisen med antall timer for å finne gjennomsnittet.

                    if (firstTotal + todaysPrices[j] > average) // ser om timeprisen pluss thresholdprosenten er høyere enn gjennomsnittlig pris.
                    {
                        houroff.objID = thresholdlist[i].id;
                        houroff.hour = j;
                        houroff.hourOn = j + 1;
                        houroff.onOff = false; // hvis den er det så skrur objektet seg av.
                    }
                    else // ellers
                    {
                        houroff.objID = thresholdlist[i].id;
                        houroff.hour = j;
                        houroff.onOff = true; // så skrur det seg på.
                    }
                    results.Add(houroff); // Her legges 24 timer med resultater inn i en resultatliste.
                    
                    Console.WriteLine($"{houroff.objID} " + $"{average} " + $"{firstTotal + todaysPrices[j]} " + $"{houroff.onOff}"); // Setter sammen informasjonen for at det skal bli forståelig
                }                                                                                                                    // for brukeren av generatoren. Den viser objektID, gjennomsnittlig pris,
            }                                                                                                                       // pris pluss threshold-pris og om systemet det gjelder er av eller på den
                                                                                                                                   // timen.
            hourofflist.AddRange(results); // Resultatlisten blir lagt inn i en liste av hele resultatet.
        }
    }
}
