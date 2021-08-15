using System;

namespace Tarla
{
    class Program
    {
        static string[] cropNames = { "buğday", "arpa", "pirinç", "mısır" };
        static int money = 0;
        static int day = 1;
        static Tile[] tiles = new Tile[5];

        //ekinler için bir sınıf
        class Crop
        {
            public string name;
            public int value;
            public Crop(string _name,int _value)
            {
                name = _name;
                value = _value;
            }
        }

        //tarla parçaları için bir sınıf
        class Tile
        {
            public Crop crop = null;
            public int stage = 0;
            public bool fertilized = false;
            public Tile()
            {
                crop = null;
                stage = 0;
                fertilized = false;
            }
        }

        //günü bitir
        static void SkipDay()
        {
            Console.WriteLine("Gün sona erdi.");
            //bitkileri büyüt
            foreach (Tile t in tiles)
            {
                if (!(t.crop==null) && t.stage < 3)
                {
                    t.stage++;
                }
            }
            day += 1;
            Console.WriteLine("\n" + day + ". gün başlıyor.");
        }

        //belli bir aralıktaki bir tamsayıya parse edilebilen bir string alana kadar
        //girdi istemeye devam eden bir girdi alma metodu
        static int IntInputInRange(string requestMessage, int min,int max)
        {
            int parsedInt=0;
            bool success=false;

            while (true)
            {
                Console.WriteLine(requestMessage);
                //string'in int'e çevirmeyi dene
                success=Int32.TryParse(Console.ReadLine(), out parsedInt);
                //eğer parse başarılı ve int doğru aralıkta ise while bitsin
                if (success && (min <= parsedInt && parsedInt <= max))
                {   
                    break;
                }
                Console.WriteLine("Girdi "+ "[" + min + "," + max + "]"+" aralığında bir tamsayı olmalı.");
            }
            return parsedInt;
        }
        //tarlanın durumu hakkında bilgi ver 
        static void DisplayField()
        {
            //para miktarını
            Console.WriteLine("--------------------------");
            Console.WriteLine(money+"TL    Gün "+day+"\n");
            //tarla parçalarını say ve durumlarını göster
            int i = 1;
            foreach (Tile t in tiles)
            {
                //eğer tarlada bir şey ekili ise
                if (!(t.crop == null))
                {
                    string cropMessage = i + ". tarlada " + t.crop.name + " ekili. [" + t.stage + "/3]";
                    //tarlanın gübreli olup olmadığını belirt
                    if (t.fertilized)
                    {
                        cropMessage += "[G]";
                    }
                    Console.WriteLine(cropMessage);
                } else
                //eğer tarlada bir şey ekili değil ise
                {
                    Console.WriteLine(i + ". tarla boş.");
                }
                i++;
            }
            Console.WriteLine("--------------------------");
        }

        static void Main()
        {
            //boş bir tarla yükle
            for (int i=0; i < 5; i++)
            {
                tiles[i] = new Tile();
            }
            //sonsuz döngü
            while (true)
            {
                Console.WriteLine("Ne yapmak istersiniz?");
                string userInput = Console.ReadLine();
                switch (userInput)
                {
                    case "uyu":
                        SkipDay();
                        break;

                    case "ek":
                        //kullanıcıya tarla parçasını seçtir
                        int tileIndex = IntInputInRange("Ekmek istediğiniz tarla sayısını seçin. [1,5]", 1, 5);
                        Console.WriteLine("Ekmek için " + tileIndex.ToString() + ". tarlayı seçtiniz.");
                        //kullanıcıya ekmek istediği bitkiyi seçtir
                        int cropIndex= IntInputInRange("Ekmek istediğiniz bitkiyi seçin.\n1)Buğday\n2)Arpa\n3)Pirinç\n4)Mısır [1,4]", 1, 4);  
                        //seçilen tarla parçasına seçilen bitkiyi ek
                        tiles[tileIndex - 1].crop = new Crop(cropNames[cropIndex - 1], 5);
                        //işlemin başarıyla bittiğini gösteren bir mesaj gönder
                        Console.WriteLine(tileIndex.ToString() + ". tarlaya " + cropNames[cropIndex - 1]+" ektiniz.");
                        break;

                    case "görüntüle":
                        DisplayField();
                        break;

                    case "hasat":
                        int harvestTileIndex = IntInputInRange("Hasat etmek istediğiniz tarla sayısı seçin.", 1, 5);
                        //eğer tarla parçası boş değil ise
                        if (!(tiles[harvestTileIndex - 1].crop == null))
                        {
                            //tarla parçasında bulunan bitkinin hasada hazır olup olmadığını ölç.
                            if (tiles[harvestTileIndex - 1].stage == 3)
                            {
                                //hasat ederken tarla pozisyonunu boşalt, bitkinin değerini paraya ekle
                                //eğer tarla parçası gübreli ise hasadın değerini arttır
                                int moneyMade = tiles[harvestTileIndex - 1].crop.value;
                                if (tiles[harvestTileIndex - 1].fertilized)
                                {
                                    moneyMade *=2;
                                }
                                money += moneyMade;
                                Console.WriteLine(moneyMade + "TL kazanıldı.");
                                tiles[harvestTileIndex - 1].crop = null;
                                tiles[harvestTileIndex - 1].fertilized = false;
                                tiles[harvestTileIndex - 1].stage = 0;
                            } else
                            {
                                Console.WriteLine("Bu bitki hasat edilmeye hazır değil.");
                            }
                        } else
                        {
                            Console.WriteLine("Bu tarla parçası boş.");
                        }
                        break;
                    case "gübre":
                        int fertilizeTileIndex = IntInputInRange("Gübre atmak istediğiniz tarla sayısı seçin. [1,5]", 1, 5);
                        if (tiles[fertilizeTileIndex - 1].fertilized)
                        {
                            Console.WriteLine("Bu tarlaya zaten gübre atılmış.");
                        } else
                        {
                            tiles[fertilizeTileIndex - 1].fertilized = true;
                            Console.WriteLine(fertilizeTileIndex + ". tarlaya gübre attınız.");
                        }
                        break;

                    case "yardım":
                        Console.WriteLine("KOMUTLAR\nTarlayı 'görüntüle'\nBitki 'ek'\nBitkileri 'hasat' et\nTarlaya 'gübre' at\n'Uyu'");
                        break;
                    default:
                        Console.WriteLine("Bilinmeyen komut.");
                        break;
                }
            }
        }

        
    }
}
