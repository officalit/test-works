using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using MySql.Data.MySqlClient;
using _09052019.Any;
using System.Threading;
using System.Windows;

namespace _09052019.Model
{
    public class MODEL_OTRS : OnPropertyChangedClass
    {
        public int очередь_олл;
        public string фио_сотрудника;
        public string номер_телефона;
        public int количество_заявок;
        public int количество_звонков;
        public int количество_баллов;
        public int количество_звонков_поступивших;
        public int количество_звонков_принятых;
        public string фио_ждшника;
        public int количество_звонков_ждшника;
        public string номер_телефона_ждшника;

        public string Номер_телефона_ждшника
        {
            get { return номер_телефона_ждшника; }
            set { номер_телефона_ждшника = value; }
        }
        public string ФИО_ждшника
        {
            get { return фио_ждшника; }
            set { фио_ждшника = value; }
        }
        public int Количество_звонков_ждшника
        {
            get { return количество_звонков_ждшника; }
            set { количество_звонков_ждшника = value; }
        }
        public string ФИО_сотрудника
        {
            get { return фио_сотрудника; }
            set
            {
                фио_сотрудника = value;
                OnPropertyChanged("фио_сотрудника");
            }
        }
        public string Номер_телефона
        {
            get { return номер_телефона; }
            set
            {
                номер_телефона = value;
                OnPropertyChanged("номер_телефона");
            }
        }
        public int Количество_заявок
        {
            get { return количество_заявок; }
            set
            {
                количество_заявок = value;
                OnPropertyChanged("количество_заявок");
            }
        }
        public int Количество_звонков
        {
            get { return количество_звонков; }
            set
            {
                количество_звонков = value;
                OnPropertyChanged("количество_звонков");
            }
        }
        public int Количество_баллов
        {
            get { return количество_баллов; }
            set
            {
                количество_баллов = value;
                OnPropertyChanged("количество_баллов");
            }
        }
        public int Количество_звонков_поступивших
        {
            get { return количество_звонков_поступивших; }
            set
            {
                количество_звонков_поступивших = value;
                OnPropertyChanged("количество_звонков_поступивших");
            }
        }
        public int Очередь_олл
        {
            get { return очередь_олл; }
            set { очередь_олл = value; }
        }



        ObservableCollection<MODEL_OTRS> данные_из_отрс;
        public ObservableCollection<MODEL_OTRS> GetДанные_из_отрс()
        {
            return данные_из_отрс;
        }

        public async Task Load()
        {
            await Task.Delay(1000);
            MySqlConnectionStringBuilder conn_string = new MySqlConnectionStringBuilder();
            conn_string.Server = "deleted";
            conn_string.UserID = "deleted";
            conn_string.Password = "deleted";
            conn_string.Database = "otrs5.1";
            данные_из_отрс = new ObservableCollection<Model.MODEL_OTRS>();
            using (MySqlConnection MyCon = new MySqlConnection(conn_string.ToString()))
            {
                await MyCon.OpenAsync();
                string query = "select * from `otrs5.1`.rbt_stats;";
                string запрос_на_ждшника = "SELECT * FROM `otrs5.1`.rbt_pod_stats;";
                string запрос_очередь_олл = "SELECT count(*) as score FROM `otrs5.1`.ticket where queue_id=5 and ticket_state_id=1";

                using (MySqlCommand command = new MySqlCommand(запрос_на_ждшника, MyCon))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (await reader.ReadAsync())
                        {
                            фио_ждшника = Convert.ToString(reader["agent_name"]);
                            номер_телефона_ждшника = Convert.ToString(reader["agent_phone"]);
                        }
                    }
                    command.Dispose();
                }

                using (MySqlCommand command = new MySqlCommand(query, MyCon))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (await reader.ReadAsync())
                        {
                            var x = new Model.MODEL_OTRS();
                            var a = Convert.ToString(reader["agent_name"]);
                            var b = Convert.ToString(reader["agent_phone"]);
                            if (a != фио_ждшника)
                            {
                                if (b == номер_телефона_ждшника)
                                {
                                    x.фио_сотрудника = a;
                                    x.номер_телефона = Convert.ToString(reader["agent_phone"]);
                                    x.количество_заявок = Convert.ToInt16(reader["ticket_count"]);
                                    x.количество_звонков = Convert.ToInt16(reader["calls_count"]) - количество_звонков_ждшника;
                                    x.количество_баллов = x.количество_звонков * 2 + x.количество_заявок;
                                    данные_из_отрс.Add(x);
                                }

                                else
                                {
                                    x.фио_сотрудника = a;
                                    x.номер_телефона = Convert.ToString(reader["agent_phone"]);
                                    x.количество_заявок = Convert.ToInt16(reader["ticket_count"]);
                                    x.количество_звонков = Convert.ToInt16(reader["calls_count"]);
                                    x.количество_баллов = x.количество_звонков * 2 + x.количество_заявок;
                                    данные_из_отрс.Add(x);

                                }
                            }
                        }
                    }
                    command.Dispose();
                }


                    using (MySqlCommand command = new MySqlCommand(запрос_очередь_олл, MyCon))
                    {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (await reader.ReadAsync())
                        {
                            var a = Convert.ToInt16(reader["score"]);
                            очередь_олл = a;
                        }
                    }
                    command.Dispose();

                }
                await MyCon.CloseAsync();
            }
            OnPropertyChanged("ИзмененаСтатистика");
        }

        public async Task Load2(string датасегодня)
        {
            await Task.Delay(3000);
            using (MySqlConnection connection = new MySqlConnection(@"deleted"))
            {
                try
                {
                    await connection.OpenAsync();
                    string запрос_на_предыдущий_день = "SELECT dst as number, Count(calldate) as score FROM cdrdb.acdr WHERE calldate like '%" + датасегодня + "%' and dcontext = 'counter' group by dst";
                    string запрос_принятые_сегодня = "SELECT dst as number, Count(calldate) as score FROM cdrdb.acdr WHERE calldate like '%" + датасегодня + "%' and disposition = 'ANSWERED' and dcontext not in ('out') and realdst not in (0);";
                    string запрос_на_ждшника = "SELECT dst as number, Count(calldate) as score FROM cdrdb.acdr WHERE calldate between " + "\"" + датасегодня + " 00:00:00" + "\"" + " and " + "\"" + датасегодня +" 08:30:00" + "\"" + " and disposition = 'ANSWERED' and dcontext not in ('out') and realdst not in (0) group by dst;";
                    using (MySqlCommand command = new MySqlCommand(запрос_принятые_сегодня, connection))
                    {
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (await reader.ReadAsync())
                            {
                                количество_звонков_принятых = Convert.ToInt16(reader["score"]);
                            }
                        }
                        command.Dispose();
                    }


                    using (MySqlCommand command = new MySqlCommand(запрос_на_предыдущий_день, connection))
                    {
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (await reader.ReadAsync())
                            {
                                количество_звонков_поступивших = Convert.ToInt16(reader["score"]);
                            }
                        }
                        command.Dispose();
                    }
                    using (MySqlCommand command = new MySqlCommand(запрос_на_ждшника, connection))
                    {
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (await reader.ReadAsync())
                            {
                                var a = Convert.ToString(reader["number"]);
                                var b = Convert.ToInt16(reader["score"]);
                                if (a == номер_телефона_ждшника)
                                {
                                    количество_звонков_ждшника = b;
                                }
                            }
                        }
                        command.Dispose();
                    }

                    await connection.CloseAsync();
                }

                catch
                {

                }

                OnPropertyChanged("ИзмененыДанныеИзТелефонии");
            }
 
        }
    }
}
