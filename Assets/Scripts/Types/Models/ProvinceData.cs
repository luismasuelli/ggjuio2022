using System;
using Newtonsoft.Json;


namespace GGJUIO2020.Types
{
    namespace Models
    {
        public class ProvinceData
        {
            public static ProvinceData[] Data = new[]
            {
                new ProvinceData()
                {
                    Name = "Manabi",
                    Cuisine = "La Tonga",
                    Regional = "La playa Murcielago",
                    Culture = "nacio Eloy Alfaro",
                },
                new ProvinceData()
                {
                    Name = "Galapagos",
                    Cuisine = "El encocado de churo",
                    Regional = "Las grietas",
                    Culture = "estuvo Charles Darwin llevando a cabo sus investigaciones",
                },
                new ProvinceData()
                {
                    Name = "Guayas",
                    Cuisine = "El maduro lampreado",
                    Regional = "La mayor cuenca hidrografica de todo el Pacifico Sur",
                    Culture = "hubo una gran cantidad de piratas",
                },
                new ProvinceData()
                {
                    Name = "Imbabura",
                    Cuisine = "La chicha de jora",
                    Regional = "El lago San Pablo",
                    Culture = "se practica la cacería del zorro",
                },
                new ProvinceData()
                {
                    Name = "Azuay",
                    Cuisine = "El mote pisho",
                    Regional = "La hidroelectrica de Paute",
                    Culture = "se encuentra la Atenas del Ecuador",
                },
                new ProvinceData()
                {
                    Name = "Loja",
                    Cuisine = "El repe",
                    Regional = "Los bosques petrificados de Puyango",
                    Culture = "se puede encontrar a la gente mas longeva del Ecuador",
                },
                new ProvinceData()
                {
                    Name = "Sucumbios",
                    Cuisine = "El ceviche de palmito",
                    Regional = "Las cascadas de San Rafael",
                    Culture = "podemos encontrar la etnia de los Cofanes",
                },
                new ProvinceData()
                {
                    Name = "Napo",
                    Cuisine = "El maito de guanta",
                    Regional = "El Yasuni",
                    Culture = "ocurrio el gran levantamiento de los pueblos indigenas de 1578",
                },
                new ProvinceData()
                {
                    Name = "Zamora Chinchipe",
                    Cuisine = "Las ancas de rana",
                    Regional = "El rio Bombuscaro",
                    Culture = "ocurrio el conflicto del Falso Paquisha",
                }
            };
            
            public string Name;
            public string Cuisine;
            public string Regional;
            public string Culture;

            public string CurrentMission(string questionType)
            {
                string baseText = "";
                switch (questionType)
                {
                    case "cuisine":
                        baseText = "Dirigete a la provincia cuya comida principal es: " + Cuisine;
                        break;
                    case "regional":
                        baseText = "Dirigete a la provincia que tiene: " + Regional;
                        break;
                    case "culture":
                        baseText = "Dirigete a la provincia en donde " + Culture;
                        break;
                    default:
                        throw new ArgumentException($"Invalid question type: {questionType}");
                }
                
                return baseText;
            }

            public string Info(string questionType)
            {
                string baseText = "";
                switch (questionType)
                {
                    case "cuisine":
                        baseText = "Aqui la comida principal es: " + Cuisine;
                        break;
                    case "regional":
                        baseText = "Aqui puedes encontrar: " + Regional;
                        break;
                    case "culture":
                        baseText = "Esta es la provincia en donde " + Culture;
                        break;
                    default:
                        throw new ArgumentException($"Invalid question type: {questionType}");
                }

                return baseText;
            }
        }
    }
}