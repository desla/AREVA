namespace Alvasoft.KPPBridge.Configuration
{
    using System.IO;
    using System.Xml.Serialization;

    /// <summary>
    /// Конфигурация серий.
    /// </summary>
    public class LinesConfiguration
    {
        /// <summary>
        /// Запрос на вставку данных.
        /// </summary>
        public string InsertQuery { get; set; }

        /// <summary>
        /// Список параметров запроса.
        /// </summary>
        public string[] Parameters { get; set; }

        /// <summary>
        /// Список серий.
        /// </summary>
        public LineConfiguration[] Lines { get; set; }

        /// <summary>
        /// Загружает конфигурацию серий из файла.
        /// </summary>
        /// <param name="aXmlFile"></param>
        /// <returns></returns>
        public static LinesConfiguration LoadFromFile(string aXmlFile)
        {
            using (var stream = new StreamReader(aXmlFile)) {
                var serializer = new XmlSerializer(typeof (LinesConfiguration));
                return (LinesConfiguration) serializer.Deserialize(stream);
            }
        }

        /// <summary>
        /// Для тестов.
        /// </summary>
        /// <param name="aXmlFile"></param>
        public static void Serialize(string aXmlFile)
        {
            var c = new LinesConfiguration {
                InsertQuery = "insert into CSDATA(" +
                                            "DATA_TIME, " +
                                            "LINE_ID, " +
                                            "LINE_CURRENT_SETPOINT, " +
                                            "LINE_CURRENT, " +
                                            "LINE_VOLTAGE, " +
                                            "POTROOM1_VOLTAGE, " +
                                            "POTROOM2_VOLTAGE, " +
                                            "POTROOM3_VOLTAGE, " +
                                            "POTROOM4_VOLTAGE, " +
                                            "POTROOM5_VOLTAGE) " +
                                      "values(:data_time, " +
                                            ":line_id, " +
                                            ":line_current_setpoint, " +
                                            ":line_current, " +
                                            ":line_voltage, " +
                                            ":potroom1_voltage, " +
                                            ":potroom2_voltage, " +
                                            ":potroom3_voltage, " +
                                            ":potroom4_voltage, " +
                                            ":potroom5_voltage)",
                Lines = new LineConfiguration[] {
                    new LineConfiguration {
                        Id = 1,
                        Interval = 60,                        
                        Parameters = new QueryParameter[] {
                            new QueryParameter {
                                Name = "line_current_setpoint",
                                OpcTag = "RTS1_SRV1.POTLINE.L3_SETPOINT"
                            },
                            new QueryParameter {
                                Name = "line_current",
                                OpcTag = "RTS1_SRV1.POTLINE.DCCURRENT"
                            },
                            new QueryParameter {
                                Name = "line_voltage",
                                OpcTag = "RTS1_SRV1.POTLINE.DCVOLTAGE"
                            },
                            new QueryParameter {
                                Name = "potroom1_voltage",
                                OpcTag = "RTS1_SRV1.G01.DCVOLTAGE"
                            },
                            new QueryParameter {
                                Name = "potroom2_voltage",
                                OpcTag = "RTS1_SRV1.G02.DCVOLTAGE"
                            },
                            new QueryParameter {
                                Name = "potroom3_voltage",
                                OpcTag = "RTS1_SRV1.G03.DCVOLTAGE"
                            },
                            new QueryParameter {
                                Name = "potroom4_voltage",
                                OpcTag = "RTS1_SRV1.G04.DCVOLTAGE"
                            },
                            new QueryParameter {
                                Name = "potroom5_voltage",
                                OpcTag = "RTS1_SRV1.G05.DCVOLTAGE"
                            },
                        }
                    }
                }
            };
            
            using (var stream = new StreamWriter(aXmlFile)) {
                try {
                    var serializer = new XmlSerializer(typeof (LinesConfiguration));
                    serializer.Serialize(stream, c);
                }
                catch (FileNotFoundException) {
                }
            }                        
        }
    }
}
