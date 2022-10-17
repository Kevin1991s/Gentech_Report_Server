using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using S7.Net;
using System.Data.SqlClient;

namespace Gentech_Report_Server
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }
        protected override void OnStart(string[] args)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            List<Task> tasks = new List<Task>
            {
                GET_H2SO4(),
            };
            tasks.Add(GET_H3PO4());
            tasks.Add(GET_HCL());
            tasks.Add(GET_HF49());
            tasks.Add(GET_HF100());
            tasks.Add(GET_CuSO4());
            tasks.Add(GET_BOE());
            tasks.Add(GET_HHC());
            tasks.Add(GET_SpinD());
            tasks.Add(GET_SC100());
            tasks.Add(GET_NH4OH());
            tasks.Add(GET_DEV_M());
            tasks.Add(GET_PlanarClean());
            tasks.Add(GET_HNO3A());
            tasks.Add(GET_HNO3B());
            tasks.Add(GET_H2O2A());
            tasks.Add(GET_H2O2B());
            tasks.Add(GET_IPAA());
            tasks.Add(GET_IPAB());
            tasks.Add(GET_OK73A());
            tasks.Add(GET_OK73B());
            tasks.Add(GET_SYS9055A());
            tasks.Add(GET_SYS9055B());
            tasks.Add(GET_SYS9058A());
            tasks.Add(GET_SYS9058B());
            tasks.Add(GET_NMPA());
            tasks.Add(GET_NMPB());

            //await Task.WhenAll(tasks);

        }


        protected override void OnStop()
        {

        }


        private static async Task GET_H2SO4()
        {
            Plc H2SO4 = new Plc(CpuType.S71500, "10.249.76.94", 0, 1);
            string BarCode;
            string SNCode;
            string ExpData;
            string SQL_Ex;
            int DrumA_lastStep;
            int DrumB_lastStep;
            int Lorry_lastStep;
            DateTime stimeA, stimeB, stimeC, stimeD;
            DrumA_lastStep = 0;
            DrumB_lastStep = 0;
            Lorry_lastStep = 0;
            stimeA = DateTime.Now;
            stimeB = DateTime.Now;
            stimeC = DateTime.Now;
            stimeD = DateTime.Now;
            while (true)
            {

                if (H2SO4.IsConnected == false)
                {
                    //Console.WriteLine("尝试连接H2SO4 PLC");
                    H2SO4.OpenAsync().Wait(2000);
                }

                if (H2SO4.IsConnected == true)
                {
                    var DrumA_Step = H2SO4.Read("DB1105.DBW0");
                    var DrumB_Step = H2SO4.Read("DB1205.DBW0");
                    if (DrumA_Step.ToString() == "3" && DrumA_lastStep == 0)
                    {
                        stimeA = DateTime.Now;
                        SQL_Ex = "exec Int_Drum '" + stimeA.ToString() + "','H2SO4','DrumA'";
                        AsyncExecuteNonQuery(SQL_Ex, CallbackAsyncExecuteNonQuery);
                        DrumA_lastStep = 2;
                    }
                    if (DrumA_Step.ToString()  == "5" && DrumA_lastStep == 2)
                    {
                        var WriteBit = H2SO4.Read("DB911.DBX292.0");
                        if (WriteBit.ToString() == "True")
                        {

                            var m_BarCode = H2SO4.ReadBytes(DataType.DataBlock, 911, 324, 40);
                            var m_SNCode = H2SO4.ReadBytes(DataType.DataBlock, 911, 364, 40);
                            var m_ExpData = H2SO4.ReadBytes(DataType.DataBlock, 911, 404, 40);

                            BarCode = System.Text.ASCIIEncoding.ASCII.GetString(m_BarCode);
                            SNCode = System.Text.ASCIIEncoding.ASCII.GetString(m_SNCode);
                            ExpData = System.Text.ASCIIEncoding.ASCII.GetString(m_ExpData);

                            // update Report_DrumN set 条码 = '',批号 = '',有效期 = '' where 更换时间 = '' and 化学品名称 = '' and Drum名称 = ''

                            SQL_Ex = "update Report_DrumN set 条码 = '" + BarCode + "',批号 = '" + SNCode + "',有效期 = '" + ExpData + "' where 更换时间 = '" + stimeA.ToString() + "' and 化学品名称 = 'H2SO4' and Drum名称 = 'DrumA'";

                            //SQL_Ex = "INSERT INTO Report_DrumN(更换时间,化学品名称,Drum名称,条码) Values('" + stime.ToString() + "','H2SO4','" + ObjectN + "','" + BarCode + "')";


                            AsyncExecuteNonQuery(SQL_Ex, CallbackAsyncExecuteNonQuery);
                            H2SO4.Write("DB911.DBX292.0", false);

                        }
                        DrumA_lastStep = 0;
                    }

                    if (DrumB_Step.ToString() == "3" && DrumB_lastStep == 0)
                    {
                        stimeB = DateTime.Now;
                        SQL_Ex = "exec Int_Drum '" + stimeB.ToString() + "','H2SO4','DrumB'";
                        AsyncExecuteNonQuery(SQL_Ex, CallbackAsyncExecuteNonQuery);
                        DrumB_lastStep = 2;
                    }
                    if (DrumB_Step.ToString()  == "5" && DrumB_lastStep == 2)
                    {
                        var WriteBit = H2SO4.Read("DB911.DBX292.0");
                        if (WriteBit.ToString() == "True")
                        {

                            var m_BarCode = H2SO4.ReadBytes(DataType.DataBlock, 911, 324, 40);
                            var m_SNCode = H2SO4.ReadBytes(DataType.DataBlock, 911, 364, 40);
                            var m_ExpData = H2SO4.ReadBytes(DataType.DataBlock, 911, 404, 40);

                            BarCode = System.Text.ASCIIEncoding.ASCII.GetString(m_BarCode);
                            SNCode = System.Text.ASCIIEncoding.ASCII.GetString(m_SNCode);
                            ExpData = System.Text.ASCIIEncoding.ASCII.GetString(m_ExpData);

                            // update Report_DrumN set 条码 = '',批号 = '',有效期 = '' where 更换时间 = '' and 化学品名称 = '' and Drum名称 = ''

                            SQL_Ex = "update Report_DrumN set 条码 = '" + BarCode + "',批号 = '" + SNCode + "',有效期 = '" + ExpData + "' where 更换时间 = '" + stimeB.ToString() + "' and 化学品名称 = 'H2SO4' and Drum名称 = 'DrumB'";

                            //SQL_Ex = "INSERT INTO Report_DrumN(更换时间,化学品名称,Drum名称,条码) Values('" + stime.ToString() + "','H2SO4','" + ObjectN + "','" + BarCode + "')";


                            AsyncExecuteNonQuery(SQL_Ex, CallbackAsyncExecuteNonQuery);
                            H2SO4.Write("DB911.DBX292.0", false);

                        }
                        DrumB_lastStep = 0;
                    }

                    var Lorry_Step = H2SO4.Read("DB5105.DBW0");
                    if (Lorry_Step.ToString() == "6" && Lorry_lastStep == 0)
                    {
                        stimeC = DateTime.Now;
                        //exec Int_Lorry '2022-06-26 15:37:20.000','H2SO4'
                        SQL_Ex = "exec Int_Lorry '" + stimeC.ToString() + "','H2SO4'";
                        AsyncExecuteNonQuery(SQL_Ex, CallbackAsyncExecuteNonQuery);
                        Lorry_lastStep = 3;
                    }
                    if (Lorry_Step.ToString() == "16" && Lorry_lastStep == 3)
                    {
                        stimeD = DateTime.Now;
                        SQL_Ex = "update Report_CCBN set 结束时间 = '" + stimeD.ToString() + "'";
                        var LWriteBit = (bool)H2SO4.Read("DB910.DBX292.0");
                        if (LWriteBit == true)
                        {
                            var m_BarCode = H2SO4.ReadBytes(DataType.DataBlock, 910, 324, 40);
                            var m_SNCode = H2SO4.ReadBytes(DataType.DataBlock, 910, 364, 40);
                            var m_ExpData = H2SO4.ReadBytes(DataType.DataBlock, 910, 404, 40);
                            BarCode = System.Text.ASCIIEncoding.ASCII.GetString(m_BarCode);
                            SNCode = System.Text.ASCIIEncoding.ASCII.GetString(m_SNCode);
                            ExpData = System.Text.ASCIIEncoding.ASCII.GetString(m_ExpData);
                            SQL_Ex = SQL_Ex + ",条码 = '" + BarCode + "',批号 = '" + SNCode + "',有效期 = '" + ExpData + "' where 化学品名称 = 'H2SO4' and 开始时间 = '" + stimeC.ToString() + "'";
                            H2SO4.Write("DB910.DBX292.0", false);

                        }
                        AsyncExecuteNonQuery(SQL_Ex, CallbackAsyncExecuteNonQuery);
                        Lorry_lastStep = 0;
                    }


                }

                await Task.Delay(1000);
            }

        }
        private static async Task GET_H3PO4()
        {
            Plc H3PO4 = new Plc(CpuType.S71500, "10.249.76.4", 0, 1);
            string BarCode;
            string SNCode;
            string ExpData;
            string SQL_Ex;
            int DrumA_lastStep;
            int DrumB_lastStep;
            int Lorry_lastStep;
            DateTime stimeA, stimeB, stimeC, stimeD;
            DrumA_lastStep = 0;
            DrumB_lastStep = 0;
            Lorry_lastStep = 0;
            stimeA = DateTime.Now;
            stimeB = DateTime.Now;
            stimeC = DateTime.Now;
            stimeD = DateTime.Now;
            while (true)
            {

                if (H3PO4.IsConnected == false)
                {
                    //Console.WriteLine("尝试连接H3PO4 PLC");
                    H3PO4.OpenAsync().Wait(2000);
                }

                if (H3PO4.IsConnected == true)
                {
                    var DrumA_Step = H3PO4.Read("DB1105.DBW0");
                    var DrumB_Step = H3PO4.Read("DB1205.DBW0");
                    if (DrumA_Step.ToString() == "3" && DrumA_lastStep == 0)
                    {
                        stimeA = DateTime.Now;
                        SQL_Ex = "exec Int_Drum '" + stimeA.ToString() + "','H3PO4','DrumA'";
                        AsyncExecuteNonQuery(SQL_Ex, CallbackAsyncExecuteNonQuery);
                        DrumA_lastStep = 2;
                    }
                    if (DrumA_Step.ToString()  == "5" && DrumA_lastStep == 2)
                    {
                        var WriteBit = H3PO4.Read("DB911.DBX292.0");
                        if (WriteBit.ToString() == "True")
                        {

                            var m_BarCode = H3PO4.ReadBytes(DataType.DataBlock, 911, 324, 40);
                            var m_SNCode = H3PO4.ReadBytes(DataType.DataBlock, 911, 364, 40);
                            var m_ExpData = H3PO4.ReadBytes(DataType.DataBlock, 911, 404, 40);

                            BarCode = System.Text.ASCIIEncoding.ASCII.GetString(m_BarCode);
                            SNCode = System.Text.ASCIIEncoding.ASCII.GetString(m_SNCode);
                            ExpData = System.Text.ASCIIEncoding.ASCII.GetString(m_ExpData);

                            // update Report_DrumN set 条码 = '',批号 = '',有效期 = '' where 更换时间 = '' and 化学品名称 = '' and Drum名称 = ''

                            SQL_Ex = "update Report_DrumN set 条码 = '" + BarCode + "',批号 = '" + SNCode + "',有效期 = '" + ExpData + "' where 更换时间 = '" + stimeA.ToString() + "' and 化学品名称 = 'H3PO4' and Drum名称 = 'DrumA'";

                            //SQL_Ex = "INSERT INTO Report_DrumN(更换时间,化学品名称,Drum名称,条码) Values('" + stime.ToString() + "','H3PO4','" + ObjectN + "','" + BarCode + "')";


                            AsyncExecuteNonQuery(SQL_Ex, CallbackAsyncExecuteNonQuery);
                            H3PO4.Write("DB911.DBX292.0", false);

                        }
                        DrumA_lastStep = 0;
                    }

                    if (DrumB_Step.ToString() == "3" && DrumB_lastStep == 0)
                    {
                        stimeB = DateTime.Now;
                        SQL_Ex = "exec Int_Drum '" + stimeB.ToString() + "','H3PO4','DrumB'";
                        AsyncExecuteNonQuery(SQL_Ex, CallbackAsyncExecuteNonQuery);
                        DrumB_lastStep = 2;
                    }
                    if (DrumB_Step.ToString()  == "5" && DrumB_lastStep == 2)
                    {
                        var WriteBit = H3PO4.Read("DB911.DBX292.0");
                        if (WriteBit.ToString() == "True")
                        {

                            var m_BarCode = H3PO4.ReadBytes(DataType.DataBlock, 911, 324, 40);
                            var m_SNCode = H3PO4.ReadBytes(DataType.DataBlock, 911, 364, 40);
                            var m_ExpData = H3PO4.ReadBytes(DataType.DataBlock, 911, 404, 40);

                            BarCode = System.Text.ASCIIEncoding.ASCII.GetString(m_BarCode);
                            SNCode = System.Text.ASCIIEncoding.ASCII.GetString(m_SNCode);
                            ExpData = System.Text.ASCIIEncoding.ASCII.GetString(m_ExpData);

                            // update Report_DrumN set 条码 = '',批号 = '',有效期 = '' where 更换时间 = '' and 化学品名称 = '' and Drum名称 = ''

                            SQL_Ex = "update Report_DrumN set 条码 = '" + BarCode + "',批号 = '" + SNCode + "',有效期 = '" + ExpData + "' where 更换时间 = '" + stimeB.ToString() + "' and 化学品名称 = 'H3PO4' and Drum名称 = 'DrumB'";

                            //SQL_Ex = "INSERT INTO Report_DrumN(更换时间,化学品名称,Drum名称,条码) Values('" + stime.ToString() + "','H3PO4','" + ObjectN + "','" + BarCode + "')";


                            AsyncExecuteNonQuery(SQL_Ex, CallbackAsyncExecuteNonQuery);
                            H3PO4.Write("DB911.DBX292.0", false);

                        }
                        DrumB_lastStep = 0;
                    }
                }

                await Task.Delay(1000);
            }

        }
        private static async Task GET_HCL()
        {
            Plc HCL = new Plc(CpuType.S71500, "10.249.76.6", 0, 1);
            string BarCode;
            string SNCode;
            string ExpData;
            string SQL_Ex;
            int DrumA_lastStep;
            int DrumB_lastStep;
            int Lorry_lastStep;
            DateTime stimeA, stimeB, stimeC, stimeD;
            DrumA_lastStep = 0;
            DrumB_lastStep = 0;
            Lorry_lastStep = 0;
            stimeA = DateTime.Now;
            stimeB = DateTime.Now;
            stimeC = DateTime.Now;
            stimeD = DateTime.Now;
            while (true)
            {

                if (HCL.IsConnected == false)
                {
                    //Console.WriteLine("尝试连接HCL PLC");
                    HCL.OpenAsync().Wait(2000);
                }

                if (HCL.IsConnected == true)
                {
                    var DrumA_Step = HCL.Read("DB1105.DBW0");
                    var DrumB_Step = HCL.Read("DB1205.DBW0");
                    if (DrumA_Step.ToString() == "3" && DrumA_lastStep == 0)
                    {
                        stimeA = DateTime.Now;
                        SQL_Ex = "exec Int_Drum '" + stimeA.ToString() + "','HCL','DrumA'";
                        AsyncExecuteNonQuery(SQL_Ex, CallbackAsyncExecuteNonQuery);
                        DrumA_lastStep = 2;
                    }
                    if (DrumA_Step.ToString()  == "5" && DrumA_lastStep == 2)
                    {
                        var WriteBit = HCL.Read("DB911.DBX292.0");
                        if (WriteBit.ToString() == "True")
                        {

                            var m_BarCode = HCL.ReadBytes(DataType.DataBlock, 911, 324, 40);
                            var m_SNCode = HCL.ReadBytes(DataType.DataBlock, 911, 364, 40);
                            var m_ExpData = HCL.ReadBytes(DataType.DataBlock, 911, 404, 40);

                            BarCode = System.Text.ASCIIEncoding.ASCII.GetString(m_BarCode);
                            SNCode = System.Text.ASCIIEncoding.ASCII.GetString(m_SNCode);
                            ExpData = System.Text.ASCIIEncoding.ASCII.GetString(m_ExpData);

                            // update Report_DrumN set 条码 = '',批号 = '',有效期 = '' where 更换时间 = '' and 化学品名称 = '' and Drum名称 = ''

                            SQL_Ex = "update Report_DrumN set 条码 = '" + BarCode + "',批号 = '" + SNCode + "',有效期 = '" + ExpData + "' where 更换时间 = '" + stimeA.ToString() + "' and 化学品名称 = 'HCL' and Drum名称 = 'DrumA'";

                            //SQL_Ex = "INSERT INTO Report_DrumN(更换时间,化学品名称,Drum名称,条码) Values('" + stime.ToString() + "','HCL','" + ObjectN + "','" + BarCode + "')";


                            AsyncExecuteNonQuery(SQL_Ex, CallbackAsyncExecuteNonQuery);
                            HCL.Write("DB911.DBX292.0", false);

                        }
                        DrumA_lastStep = 0;
                    }

                    if (DrumB_Step.ToString() == "3" && DrumB_lastStep == 0)
                    {
                        stimeB = DateTime.Now;
                        SQL_Ex = "exec Int_Drum '" + stimeB.ToString() + "','HCL','DrumB'";
                        AsyncExecuteNonQuery(SQL_Ex, CallbackAsyncExecuteNonQuery);
                        DrumB_lastStep = 2;
                    }
                    if (DrumB_Step.ToString()  == "5" && DrumB_lastStep == 2)
                    {
                        var WriteBit = HCL.Read("DB911.DBX292.0");
                        if (WriteBit.ToString() == "True")
                        {

                            var m_BarCode = HCL.ReadBytes(DataType.DataBlock, 911, 324, 40);
                            var m_SNCode = HCL.ReadBytes(DataType.DataBlock, 911, 364, 40);
                            var m_ExpData = HCL.ReadBytes(DataType.DataBlock, 911, 404, 40);

                            BarCode = System.Text.ASCIIEncoding.ASCII.GetString(m_BarCode);
                            SNCode = System.Text.ASCIIEncoding.ASCII.GetString(m_SNCode);
                            ExpData = System.Text.ASCIIEncoding.ASCII.GetString(m_ExpData);

                            // update Report_DrumN set 条码 = '',批号 = '',有效期 = '' where 更换时间 = '' and 化学品名称 = '' and Drum名称 = ''

                            SQL_Ex = "update Report_DrumN set 条码 = '" + BarCode + "',批号 = '" + SNCode + "',有效期 = '" + ExpData + "' where 更换时间 = '" + stimeB.ToString() + "' and 化学品名称 = 'HCL' and Drum名称 = 'DrumB'";

                            //SQL_Ex = "INSERT INTO Report_DrumN(更换时间,化学品名称,Drum名称,条码) Values('" + stime.ToString() + "','HCL','" + ObjectN + "','" + BarCode + "')";


                            AsyncExecuteNonQuery(SQL_Ex, CallbackAsyncExecuteNonQuery);
                            HCL.Write("DB911.DBX292.0", false);

                        }
                        DrumB_lastStep = 0;
                    }

                }

                await Task.Delay(1000);
            }

        }
        private static async Task GET_HF49()
        {
            Plc HF49 = new Plc(CpuType.S71500, "10.249.76.8", 0, 1);
            string BarCode;
            string SNCode;
            string ExpData;
            string SQL_Ex;
            int DrumA_lastStep;
            int DrumB_lastStep;
            int Lorry_lastStep;
            DateTime stimeA, stimeB, stimeC, stimeD;
            DrumA_lastStep = 0;
            DrumB_lastStep = 0;
            Lorry_lastStep = 0;
            stimeA = DateTime.Now;
            stimeB = DateTime.Now;
            stimeC = DateTime.Now;
            stimeD = DateTime.Now;
            while (true)
            {

                if (HF49.IsConnected == false)
                {
                    //Console.WriteLine("尝试连接HF49 PLC");
                    HF49.OpenAsync().Wait(2000);
                }

                if (HF49.IsConnected == true)
                {
                    var DrumA_Step = HF49.Read("DB1105.DBW0");
                    var DrumB_Step = HF49.Read("DB1205.DBW0");
                    if (DrumA_Step.ToString() == "3" && DrumA_lastStep == 0)
                    {
                        stimeA = DateTime.Now;
                        SQL_Ex = "exec Int_Drum '" + stimeA.ToString() + "','49%HF','DrumA'";
                        AsyncExecuteNonQuery(SQL_Ex, CallbackAsyncExecuteNonQuery);
                        DrumA_lastStep = 2;
                    }
                    if (DrumA_Step.ToString()  == "5" && DrumA_lastStep == 2)
                    {
                        var WriteBit = HF49.Read("DB911.DBX292.0");
                        if (WriteBit.ToString() == "True")
                        {

                            var m_BarCode = HF49.ReadBytes(DataType.DataBlock, 911, 324, 40);
                            var m_SNCode = HF49.ReadBytes(DataType.DataBlock, 911, 364, 40);
                            var m_ExpData = HF49.ReadBytes(DataType.DataBlock, 911, 404, 40);

                            BarCode = System.Text.ASCIIEncoding.ASCII.GetString(m_BarCode);
                            SNCode = System.Text.ASCIIEncoding.ASCII.GetString(m_SNCode);
                            ExpData = System.Text.ASCIIEncoding.ASCII.GetString(m_ExpData);

                            // update Report_DrumN set 条码 = '',批号 = '',有效期 = '' where 更换时间 = '' and 化学品名称 = '' and Drum名称 = ''

                            SQL_Ex = "update Report_DrumN set 条码 = '" + BarCode + "',批号 = '" + SNCode + "',有效期 = '" + ExpData + "' where 更换时间 = '" + stimeA.ToString() + "' and 化学品名称 = '49%HF' and Drum名称 = 'DrumA'";

                            //SQL_Ex = "INSERT INTO Report_DrumN(更换时间,化学品名称,Drum名称,条码) Values('" + stime.ToString() + "','49%HF','" + ObjectN + "','" + BarCode + "')";


                            AsyncExecuteNonQuery(SQL_Ex, CallbackAsyncExecuteNonQuery);
                            HF49.Write("DB911.DBX292.0", false);

                        }
                        DrumA_lastStep = 0;
                    }

                    if (DrumB_Step.ToString() == "3" && DrumB_lastStep == 0)
                    {
                        stimeB = DateTime.Now;
                        SQL_Ex = "exec Int_Drum '" + stimeB.ToString() + "','49%HF','DrumB'";
                        AsyncExecuteNonQuery(SQL_Ex, CallbackAsyncExecuteNonQuery);
                        DrumB_lastStep = 2;
                    }
                    if (DrumB_Step.ToString()  == "5" && DrumB_lastStep == 2)
                    {
                        var WriteBit = HF49.Read("DB911.DBX292.0");
                        if (WriteBit.ToString() == "True")
                        {

                            var m_BarCode = HF49.ReadBytes(DataType.DataBlock, 911, 324, 40);
                            var m_SNCode = HF49.ReadBytes(DataType.DataBlock, 911, 364, 40);
                            var m_ExpData = HF49.ReadBytes(DataType.DataBlock, 911, 404, 40);

                            BarCode = System.Text.ASCIIEncoding.ASCII.GetString(m_BarCode);
                            SNCode = System.Text.ASCIIEncoding.ASCII.GetString(m_SNCode);
                            ExpData = System.Text.ASCIIEncoding.ASCII.GetString(m_ExpData);

                            // update Report_DrumN set 条码 = '',批号 = '',有效期 = '' where 更换时间 = '' and 化学品名称 = '' and Drum名称 = ''

                            SQL_Ex = "update Report_DrumN set 条码 = '" + BarCode + "',批号 = '" + SNCode + "',有效期 = '" + ExpData + "' where 更换时间 = '" + stimeB.ToString() + "' and 化学品名称 = '49%HF' and Drum名称 = 'DrumB'";

                            //SQL_Ex = "INSERT INTO Report_DrumN(更换时间,化学品名称,Drum名称,条码) Values('" + stime.ToString() + "','49%HF','" + ObjectN + "','" + BarCode + "')";


                            AsyncExecuteNonQuery(SQL_Ex, CallbackAsyncExecuteNonQuery);
                            HF49.Write("DB911.DBX292.0", false);

                        }
                        DrumB_lastStep = 0;
                    }
                    var Lorry_Step = HF49.Read("DB5105.DBW0");
                    if (Lorry_Step.ToString() == "6" && Lorry_lastStep == 0)
                    {
                        stimeC = DateTime.Now;
                        //exec Int_Lorry '2022-06-26 15:37:20.000','49%HF'
                        SQL_Ex = "exec Int_Lorry '" + stimeC.ToString() + "','49%HF'";
                        AsyncExecuteNonQuery(SQL_Ex, CallbackAsyncExecuteNonQuery);
                        Lorry_lastStep = 3;
                    }
                    if (Lorry_Step.ToString() == "16" && Lorry_lastStep == 3)
                    {
                        stimeD = DateTime.Now;
                        SQL_Ex = "update Report_CCBN set 结束时间 = '" + stimeD.ToString() + "'";
                        var LWriteBit = (bool)HF49.Read("DB910.DBX292.0");
                        if (LWriteBit == true)
                        {
                            var m_BarCode = HF49.ReadBytes(DataType.DataBlock, 910, 324, 40);
                            var m_SNCode = HF49.ReadBytes(DataType.DataBlock, 910, 364, 40);
                            var m_ExpData = HF49.ReadBytes(DataType.DataBlock, 910, 404, 40);
                            BarCode = System.Text.ASCIIEncoding.ASCII.GetString(m_BarCode);
                            SNCode = System.Text.ASCIIEncoding.ASCII.GetString(m_SNCode);
                            ExpData = System.Text.ASCIIEncoding.ASCII.GetString(m_ExpData);
                            SQL_Ex = SQL_Ex + ",条码 = '" + BarCode + "',批号 = '" + SNCode + "',有效期 = '" + ExpData + "' where 化学品名称 = '49%HF' and 开始时间 = '" + stimeC.ToString() + "'";
                            HF49.Write("DB910.DBX292.0", false);

                        }
                        AsyncExecuteNonQuery(SQL_Ex, CallbackAsyncExecuteNonQuery);
                        Lorry_lastStep = 0;
                    }

                }

                await Task.Delay(1000);
            }

        }
        private static async Task GET_HF100()
        {
            Plc HF100 = new Plc(CpuType.S71500, "10.249.76.10", 0, 1);
            string BarCode;
            string SNCode;
            string ExpData;
            string SQL_Ex;
            int DrumA_lastStep;
            int DrumB_lastStep;
            int Lorry_lastStep;
            DateTime stimeA, stimeB, stimeC, stimeD;
            DrumA_lastStep = 0;
            DrumB_lastStep = 0;
            Lorry_lastStep = 0;
            stimeA = DateTime.Now;
            stimeB = DateTime.Now;
            stimeC = DateTime.Now;
            stimeD = DateTime.Now;
            while (true)
            {

                if (HF100.IsConnected == false)
                {
                    //Console.WriteLine("尝试连接HF100 PLC");
                    HF100.OpenAsync().Wait(2000);
                }

                if (HF100.IsConnected == true)
                {
                    var DrumA_Step = HF100.Read("DB1105.DBW0");
                    var DrumB_Step = HF100.Read("DB1205.DBW0");
                    if (DrumA_Step.ToString() == "3" && DrumA_lastStep == 0)
                    {
                        stimeA = DateTime.Now;
                        SQL_Ex = "exec Int_Drum '" + stimeA.ToString() + "','HF100','DrumA'";
                        AsyncExecuteNonQuery(SQL_Ex, CallbackAsyncExecuteNonQuery);
                        DrumA_lastStep = 2;
                    }
                    if (DrumA_Step.ToString()  == "5" && DrumA_lastStep == 2)
                    {
                        var WriteBit = HF100.Read("DB911.DBX292.0");
                        if (WriteBit.ToString() == "True")
                        {

                            var m_BarCode = HF100.ReadBytes(DataType.DataBlock, 911, 324, 40);
                            var m_SNCode = HF100.ReadBytes(DataType.DataBlock, 911, 364, 40);
                            var m_ExpData = HF100.ReadBytes(DataType.DataBlock, 911, 404, 40);

                            BarCode = System.Text.ASCIIEncoding.ASCII.GetString(m_BarCode);
                            SNCode = System.Text.ASCIIEncoding.ASCII.GetString(m_SNCode);
                            ExpData = System.Text.ASCIIEncoding.ASCII.GetString(m_ExpData);

                            // update Report_DrumN set 条码 = '',批号 = '',有效期 = '' where 更换时间 = '' and 化学品名称 = '' and Drum名称 = ''

                            SQL_Ex = "update Report_DrumN set 条码 = '" + BarCode + "',批号 = '" + SNCode + "',有效期 = '" + ExpData + "' where 更换时间 = '" + stimeA.ToString() + "' and 化学品名称 = 'HF100:1' and Drum名称 = 'DrumA'";

                            //SQL_Ex = "INSERT INTO Report_DrumN(更换时间,化学品名称,Drum名称,条码) Values('" + stime.ToString() + "','HF100:1','" + ObjectN + "','" + BarCode + "')";


                            AsyncExecuteNonQuery(SQL_Ex, CallbackAsyncExecuteNonQuery);
                            HF100.Write("DB911.DBX292.0", false);

                        }
                        DrumA_lastStep = 0;
                    }

                    if (DrumB_Step.ToString() == "3" && DrumB_lastStep == 0)
                    {
                        stimeB = DateTime.Now;
                        SQL_Ex = "exec Int_Drum '" + stimeB.ToString() + "','HF100:1','DrumB'";
                        AsyncExecuteNonQuery(SQL_Ex, CallbackAsyncExecuteNonQuery);
                        DrumB_lastStep = 2;
                    }
                    if (DrumB_Step.ToString()  == "5" && DrumB_lastStep == 2)
                    {
                        var WriteBit = HF100.Read("DB911.DBX292.0");
                        if (WriteBit.ToString() == "True")
                        {

                            var m_BarCode = HF100.ReadBytes(DataType.DataBlock, 911, 324, 40);
                            var m_SNCode = HF100.ReadBytes(DataType.DataBlock, 911, 364, 40);
                            var m_ExpData = HF100.ReadBytes(DataType.DataBlock, 911, 404, 40);

                            BarCode = System.Text.ASCIIEncoding.ASCII.GetString(m_BarCode);
                            SNCode = System.Text.ASCIIEncoding.ASCII.GetString(m_SNCode);
                            ExpData = System.Text.ASCIIEncoding.ASCII.GetString(m_ExpData);

                            // update Report_DrumN set 条码 = '',批号 = '',有效期 = '' where 更换时间 = '' and 化学品名称 = '' and Drum名称 = ''

                            SQL_Ex = "update Report_DrumN set 条码 = '" + BarCode + "',批号 = '" + SNCode + "',有效期 = '" + ExpData + "' where 更换时间 = '" + stimeB.ToString() + "' and 化学品名称 = 'HF100:1' and Drum名称 = 'DrumB'";

                            //SQL_Ex = "INSERT INTO Report_DrumN(更换时间,化学品名称,Drum名称,条码) Values('" + stime.ToString() + "','HF100:1','" + ObjectN + "','" + BarCode + "')";


                            AsyncExecuteNonQuery(SQL_Ex, CallbackAsyncExecuteNonQuery);
                            HF100.Write("DB911.DBX292.0", false);

                        }
                        DrumB_lastStep = 0;
                    }

                }

                await Task.Delay(1000);
            }

        }
        private static async Task GET_CuSO4()
        {
            Plc CuSO4 = new Plc(CpuType.S71500, "10.249.76.12", 0, 1);
            string BarCode;
            string SNCode;
            string ExpData;
            string SQL_Ex;
            int DrumA_lastStep;
            int DrumB_lastStep;
            int Lorry_lastStep;
            DateTime stimeA, stimeB, stimeC, stimeD;
            DrumA_lastStep = 0;
            DrumB_lastStep = 0;
            Lorry_lastStep = 0;
            stimeA = DateTime.Now;
            stimeB = DateTime.Now;
            stimeC = DateTime.Now;
            stimeD = DateTime.Now;
            while (true)
            {

                if (CuSO4.IsConnected == false)
                {
                    //Console.WriteLine("尝试连接CuSO4 PLC");
                    CuSO4.OpenAsync().Wait(2000);
                }

                if (CuSO4.IsConnected == true)
                {
                    var DrumA_Step = CuSO4.Read("DB1105.DBW0");
                    var DrumB_Step = CuSO4.Read("DB1205.DBW0");
                    if (DrumA_Step.ToString() == "3" && DrumA_lastStep == 0)
                    {
                        stimeA = DateTime.Now;
                        SQL_Ex = "exec Int_Drum '" + stimeA.ToString() + "','CuSO4','DrumA'";
                        AsyncExecuteNonQuery(SQL_Ex, CallbackAsyncExecuteNonQuery);
                        DrumA_lastStep = 2;
                    }
                    if (DrumA_Step.ToString()  == "5" && DrumA_lastStep == 2)
                    {
                        var WriteBit = CuSO4.Read("DB911.DBX292.0");
                        if (WriteBit.ToString() == "True")
                        {

                            var m_BarCode = CuSO4.ReadBytes(DataType.DataBlock, 911, 324, 40);
                            var m_SNCode = CuSO4.ReadBytes(DataType.DataBlock, 911, 364, 40);
                            var m_ExpData = CuSO4.ReadBytes(DataType.DataBlock, 911, 404, 40);

                            BarCode = System.Text.ASCIIEncoding.ASCII.GetString(m_BarCode);
                            SNCode = System.Text.ASCIIEncoding.ASCII.GetString(m_SNCode);
                            ExpData = System.Text.ASCIIEncoding.ASCII.GetString(m_ExpData);

                            // update Report_DrumN set 条码 = '',批号 = '',有效期 = '' where 更换时间 = '' and 化学品名称 = '' and Drum名称 = ''

                            SQL_Ex = "update Report_DrumN set 条码 = '" + BarCode + "',批号 = '" + SNCode + "',有效期 = '" + ExpData + "' where 更换时间 = '" + stimeA.ToString() + "' and 化学品名称 = 'CuSO4' and Drum名称 = 'DrumA'";

                            //SQL_Ex = "INSERT INTO Report_DrumN(更换时间,化学品名称,Drum名称,条码) Values('" + stime.ToString() + "','CuSO4','" + ObjectN + "','" + BarCode + "')";


                            AsyncExecuteNonQuery(SQL_Ex, CallbackAsyncExecuteNonQuery);
                            CuSO4.Write("DB911.DBX292.0", false);

                        }
                        DrumA_lastStep = 0;
                    }

                    if (DrumB_Step.ToString() == "3" && DrumB_lastStep == 0)
                    {
                        stimeB = DateTime.Now;
                        SQL_Ex = "exec Int_Drum '" + stimeB.ToString() + "','CuSO4','DrumB'";
                        AsyncExecuteNonQuery(SQL_Ex, CallbackAsyncExecuteNonQuery);
                        DrumB_lastStep = 2;
                    }
                    if (DrumB_Step.ToString()  == "5" && DrumB_lastStep == 2)
                    {
                        var WriteBit = CuSO4.Read("DB911.DBX292.0");
                        if (WriteBit.ToString() == "True")
                        {

                            var m_BarCode = CuSO4.ReadBytes(DataType.DataBlock, 911, 324, 40);
                            var m_SNCode = CuSO4.ReadBytes(DataType.DataBlock, 911, 364, 40);
                            var m_ExpData = CuSO4.ReadBytes(DataType.DataBlock, 911, 404, 40);

                            BarCode = System.Text.ASCIIEncoding.ASCII.GetString(m_BarCode);
                            SNCode = System.Text.ASCIIEncoding.ASCII.GetString(m_SNCode);
                            ExpData = System.Text.ASCIIEncoding.ASCII.GetString(m_ExpData);

                            // update Report_DrumN set 条码 = '',批号 = '',有效期 = '' where 更换时间 = '' and 化学品名称 = '' and Drum名称 = ''

                            SQL_Ex = "update Report_DrumN set 条码 = '" + BarCode + "',批号 = '" + SNCode + "',有效期 = '" + ExpData + "' where 更换时间 = '" + stimeB.ToString() + "' and 化学品名称 = 'CuSO4' and Drum名称 = 'DrumB'";

                            //SQL_Ex = "INSERT INTO Report_DrumN(更换时间,化学品名称,Drum名称,条码) Values('" + stime.ToString() + "','CuSO4','" + ObjectN + "','" + BarCode + "')";


                            AsyncExecuteNonQuery(SQL_Ex, CallbackAsyncExecuteNonQuery);
                            CuSO4.Write("DB911.DBX292.0", false);

                        }
                        DrumB_lastStep = 0;
                    }

                }

                await Task.Delay(1000);
            }

        }
        private static async Task GET_BOE()
        {
            Plc BOE = new Plc(CpuType.S71500, "10.249.76.14", 0, 1);
            string BarCode;
            string SNCode;
            string ExpData;
            string SQL_Ex;
            int DrumA_lastStep;
            int DrumB_lastStep;
            int Lorry_lastStep;
            DateTime stimeA, stimeB, stimeC, stimeD;
            DrumA_lastStep = 0;
            DrumB_lastStep = 0;
            Lorry_lastStep = 0;
            stimeA = DateTime.Now;
            stimeB = DateTime.Now;
            stimeC = DateTime.Now;
            stimeD = DateTime.Now;
            while (true)
            {

                if (BOE.IsConnected == false)
                {
                    //Console.WriteLine("尝试连接BOE PLC");
                    BOE.OpenAsync().Wait(2000);
                }

                if (BOE.IsConnected == true)
                {
                    var DrumA_Step = BOE.Read("DB1105.DBW0");
                    var DrumB_Step = BOE.Read("DB1205.DBW0");
                    if (DrumA_Step.ToString() == "3" && DrumA_lastStep == 0)
                    {
                        stimeA = DateTime.Now;
                        SQL_Ex = "exec Int_Drum '" + stimeA.ToString() + "','BOE130:1:7','DrumA'";
                        AsyncExecuteNonQuery(SQL_Ex, CallbackAsyncExecuteNonQuery);
                        DrumA_lastStep = 2;
                    }
                    if (DrumA_Step.ToString()  == "5" && DrumA_lastStep == 2)
                    {
                        var WriteBit = BOE.Read("DB911.DBX292.0");
                        if (WriteBit.ToString() == "True")
                        {

                            var m_BarCode = BOE.ReadBytes(DataType.DataBlock, 911, 324, 40);
                            var m_SNCode = BOE.ReadBytes(DataType.DataBlock, 911, 364, 40);
                            var m_ExpData = BOE.ReadBytes(DataType.DataBlock, 911, 404, 40);

                            BarCode = System.Text.ASCIIEncoding.ASCII.GetString(m_BarCode);
                            SNCode = System.Text.ASCIIEncoding.ASCII.GetString(m_SNCode);
                            ExpData = System.Text.ASCIIEncoding.ASCII.GetString(m_ExpData);

                            // update Report_DrumN set 条码 = '',批号 = '',有效期 = '' where 更换时间 = '' and 化学品名称 = '' and Drum名称 = ''

                            SQL_Ex = "update Report_DrumN set 条码 = '" + BarCode + "',批号 = '" + SNCode + "',有效期 = '" + ExpData + "' where 更换时间 = '" + stimeA.ToString() + "' and 化学品名称 = 'BOE130:1:7' and Drum名称 = 'DrumA'";

                            //SQL_Ex = "INSERT INTO Report_DrumN(更换时间,化学品名称,Drum名称,条码) Values('" + stime.ToString() + "','BOE130:1:7','" + ObjectN + "','" + BarCode + "')";


                            AsyncExecuteNonQuery(SQL_Ex, CallbackAsyncExecuteNonQuery);
                            BOE.Write("DB911.DBX292.0", false);

                        }
                        DrumA_lastStep = 0;
                    }

                    if (DrumB_Step.ToString() == "3" && DrumB_lastStep == 0)
                    {
                        stimeB = DateTime.Now;
                        SQL_Ex = "exec Int_Drum '" + stimeB.ToString() + "','BOE130:1:7','DrumB'";
                        AsyncExecuteNonQuery(SQL_Ex, CallbackAsyncExecuteNonQuery);
                        DrumB_lastStep = 2;
                    }
                    if (DrumB_Step.ToString()  == "5" && DrumB_lastStep == 2)
                    {
                        var WriteBit = BOE.Read("DB911.DBX292.0");
                        if (WriteBit.ToString() == "True")
                        {

                            var m_BarCode = BOE.ReadBytes(DataType.DataBlock, 911, 324, 40);
                            var m_SNCode = BOE.ReadBytes(DataType.DataBlock, 911, 364, 40);
                            var m_ExpData = BOE.ReadBytes(DataType.DataBlock, 911, 404, 40);

                            BarCode = System.Text.ASCIIEncoding.ASCII.GetString(m_BarCode);
                            SNCode = System.Text.ASCIIEncoding.ASCII.GetString(m_SNCode);
                            ExpData = System.Text.ASCIIEncoding.ASCII.GetString(m_ExpData);

                            // update Report_DrumN set 条码 = '',批号 = '',有效期 = '' where 更换时间 = '' and 化学品名称 = '' and Drum名称 = ''

                            SQL_Ex = "update Report_DrumN set 条码 = '" + BarCode + "',批号 = '" + SNCode + "',有效期 = '" + ExpData + "' where 更换时间 = '" + stimeB.ToString() + "' and 化学品名称 = 'BOE130:1:7' and Drum名称 = 'DrumB'";

                            //SQL_Ex = "INSERT INTO Report_DrumN(更换时间,化学品名称,Drum名称,条码) Values('" + stime.ToString() + "','BOE130:1:7','" + ObjectN + "','" + BarCode + "')";


                            AsyncExecuteNonQuery(SQL_Ex, CallbackAsyncExecuteNonQuery);
                            BOE.Write("DB911.DBX292.0", false);

                        }
                        DrumB_lastStep = 0;
                    }

                }

                await Task.Delay(1000);
            }

        }
        private static async Task GET_HHC()
        {
            Plc HHC = new Plc(CpuType.S71500, "10.249.76.16", 0, 1);
            string BarCode;
            string SNCode;
            string ExpData;
            string SQL_Ex;
            int DrumA_lastStep;
            int DrumB_lastStep;
            int Lorry_lastStep;
            DateTime stimeA, stimeB, stimeC, stimeD;
            DrumA_lastStep = 0;
            DrumB_lastStep = 0;
            Lorry_lastStep = 0;
            stimeA = DateTime.Now;
            stimeB = DateTime.Now;
            stimeC = DateTime.Now;
            stimeD = DateTime.Now;
            while (true)
            {

                if (HHC.IsConnected == false)
                {
                    //Console.WriteLine("尝试连接HHC PLC");
                    HHC.OpenAsync().Wait(2000);
                }

                if (HHC.IsConnected == true)
                {
                    var DrumA_Step = HHC.Read("DB1105.DBW0");
                    var DrumB_Step = HHC.Read("DB1205.DBW0");
                    if (DrumA_Step.ToString() == "3" && DrumA_lastStep == 0)
                    {
                        stimeA = DateTime.Now;
                        SQL_Ex = "exec Int_Drum '" + stimeA.ToString() + "','HHC','DrumA'";
                        AsyncExecuteNonQuery(SQL_Ex, CallbackAsyncExecuteNonQuery);
                        DrumA_lastStep = 2;
                    }
                    if (DrumA_Step.ToString()  == "5" && DrumA_lastStep == 2)
                    {
                        var WriteBit = HHC.Read("DB911.DBX292.0");
                        if (WriteBit.ToString() == "True")
                        {

                            var m_BarCode = HHC.ReadBytes(DataType.DataBlock, 911, 324, 40);
                            var m_SNCode = HHC.ReadBytes(DataType.DataBlock, 911, 364, 40);
                            var m_ExpData = HHC.ReadBytes(DataType.DataBlock, 911, 404, 40);

                            BarCode = System.Text.ASCIIEncoding.ASCII.GetString(m_BarCode);
                            SNCode = System.Text.ASCIIEncoding.ASCII.GetString(m_SNCode);
                            ExpData = System.Text.ASCIIEncoding.ASCII.GetString(m_ExpData);

                            // update Report_DrumN set 条码 = '',批号 = '',有效期 = '' where 更换时间 = '' and 化学品名称 = '' and Drum名称 = ''

                            SQL_Ex = "update Report_DrumN set 条码 = '" + BarCode + "',批号 = '" + SNCode + "',有效期 = '" + ExpData + "' where 更换时间 = '" + stimeA.ToString() + "' and 化学品名称 = 'HHC' and Drum名称 = 'DrumA'";

                            //SQL_Ex = "INSERT INTO Report_DrumN(更换时间,化学品名称,Drum名称,条码) Values('" + stime.ToString() + "','HHC','" + ObjectN + "','" + BarCode + "')";


                            AsyncExecuteNonQuery(SQL_Ex, CallbackAsyncExecuteNonQuery);
                            HHC.Write("DB911.DBX292.0", false);

                        }
                        DrumA_lastStep = 0;
                    }

                    if (DrumB_Step.ToString() == "3" && DrumB_lastStep == 0)
                    {
                        stimeB = DateTime.Now;
                        SQL_Ex = "exec Int_Drum '" + stimeB.ToString() + "','HHC','DrumB'";
                        AsyncExecuteNonQuery(SQL_Ex, CallbackAsyncExecuteNonQuery);
                        DrumB_lastStep = 2;
                    }
                    if (DrumB_Step.ToString()  == "5" && DrumB_lastStep == 2)
                    {
                        var WriteBit = HHC.Read("DB911.DBX292.0");
                        if (WriteBit.ToString() == "True")
                        {

                            var m_BarCode = HHC.ReadBytes(DataType.DataBlock, 911, 324, 40);
                            var m_SNCode = HHC.ReadBytes(DataType.DataBlock, 911, 364, 40);
                            var m_ExpData = HHC.ReadBytes(DataType.DataBlock, 911, 404, 40);

                            BarCode = System.Text.ASCIIEncoding.ASCII.GetString(m_BarCode);
                            SNCode = System.Text.ASCIIEncoding.ASCII.GetString(m_SNCode);
                            ExpData = System.Text.ASCIIEncoding.ASCII.GetString(m_ExpData);

                            // update Report_DrumN set 条码 = '',批号 = '',有效期 = '' where 更换时间 = '' and 化学品名称 = '' and Drum名称 = ''

                            SQL_Ex = "update Report_DrumN set 条码 = '" + BarCode + "',批号 = '" + SNCode + "',有效期 = '" + ExpData + "' where 更换时间 = '" + stimeB.ToString() + "' and 化学品名称 = 'HHC' and Drum名称 = 'DrumB'";

                            //SQL_Ex = "INSERT INTO Report_DrumN(更换时间,化学品名称,Drum名称,条码) Values('" + stime.ToString() + "','HHC','" + ObjectN + "','" + BarCode + "')";


                            AsyncExecuteNonQuery(SQL_Ex, CallbackAsyncExecuteNonQuery);
                            HHC.Write("DB911.DBX292.0", false);

                        }
                        DrumB_lastStep = 0;
                    }                                        
                }

                await Task.Delay(1000);
            }

        }
        private static async Task GET_SpinD()
        {
            Plc SpinD = new Plc(CpuType.S71500, "10.249.76.18", 0, 1);
            string BarCode;
            string SNCode;
            string ExpData;
            string SQL_Ex;
            int DrumA_lastStep;
            int DrumB_lastStep;
            int Lorry_lastStep;
            DateTime stimeA, stimeB, stimeC, stimeD;
            DrumA_lastStep = 0;
            DrumB_lastStep = 0;
            Lorry_lastStep = 0;
            stimeA = DateTime.Now;
            stimeB = DateTime.Now;
            stimeC = DateTime.Now;
            stimeD = DateTime.Now;
            while (true)
            {

                if (SpinD.IsConnected == false)
                {
                    //Console.WriteLine("尝试连接SpinD PLC");
                    SpinD.OpenAsync().Wait(2000);
                }

                if (SpinD.IsConnected == true)
                {
                    var DrumA_Step = SpinD.Read("DB1105.DBW0");
                    var DrumB_Step = SpinD.Read("DB1205.DBW0");
                    if (DrumA_Step.ToString() == "3" && DrumA_lastStep == 0)
                    {
                        stimeA = DateTime.Now;
                        SQL_Ex = "exec Int_Drum '" + stimeA.ToString() + "','Spin-D','DrumA'";
                        AsyncExecuteNonQuery(SQL_Ex, CallbackAsyncExecuteNonQuery);
                        DrumA_lastStep = 2;
                    }
                    if (DrumA_Step.ToString()  == "5" && DrumA_lastStep == 2)
                    {
                        var WriteBit = SpinD.Read("DB911.DBX292.0");
                        if (WriteBit.ToString() == "True")
                        {

                            var m_BarCode = SpinD.ReadBytes(DataType.DataBlock, 911, 324, 40);
                            var m_SNCode = SpinD.ReadBytes(DataType.DataBlock, 911, 364, 40);
                            var m_ExpData = SpinD.ReadBytes(DataType.DataBlock, 911, 404, 40);

                            BarCode = System.Text.ASCIIEncoding.ASCII.GetString(m_BarCode);
                            SNCode = System.Text.ASCIIEncoding.ASCII.GetString(m_SNCode);
                            ExpData = System.Text.ASCIIEncoding.ASCII.GetString(m_ExpData);

                            // update Report_DrumN set 条码 = '',批号 = '',有效期 = '' where 更换时间 = '' and 化学品名称 = '' and Drum名称 = ''

                            SQL_Ex = "update Report_DrumN set 条码 = '" + BarCode + "',批号 = '" + SNCode + "',有效期 = '" + ExpData + "' where 更换时间 = '" + stimeA.ToString() + "' and 化学品名称 = 'Spin-D' and Drum名称 = 'DrumA'";

                            //SQL_Ex = "INSERT INTO Report_DrumN(更换时间,化学品名称,Drum名称,条码) Values('" + stime.ToString() + "','Spin-D','" + ObjectN + "','" + BarCode + "')";


                            AsyncExecuteNonQuery(SQL_Ex, CallbackAsyncExecuteNonQuery);
                            SpinD.Write("DB911.DBX292.0", false);

                        }
                        DrumA_lastStep = 0;
                    }

                    if (DrumB_Step.ToString() == "3" && DrumB_lastStep == 0)
                    {
                        stimeB = DateTime.Now;
                        SQL_Ex = "exec Int_Drum '" + stimeB.ToString() + "','Spin-D','DrumB'";
                        AsyncExecuteNonQuery(SQL_Ex, CallbackAsyncExecuteNonQuery);
                        DrumB_lastStep = 2;
                    }
                    if (DrumB_Step.ToString()  == "5" && DrumB_lastStep == 2)
                    {
                        var WriteBit = SpinD.Read("DB911.DBX292.0");
                        if (WriteBit.ToString() == "True")
                        {

                            var m_BarCode = SpinD.ReadBytes(DataType.DataBlock, 911, 324, 40);
                            var m_SNCode = SpinD.ReadBytes(DataType.DataBlock, 911, 364, 40);
                            var m_ExpData = SpinD.ReadBytes(DataType.DataBlock, 911, 404, 40);

                            BarCode = System.Text.ASCIIEncoding.ASCII.GetString(m_BarCode);
                            SNCode = System.Text.ASCIIEncoding.ASCII.GetString(m_SNCode);
                            ExpData = System.Text.ASCIIEncoding.ASCII.GetString(m_ExpData);

                            // update Report_DrumN set 条码 = '',批号 = '',有效期 = '' where 更换时间 = '' and 化学品名称 = '' and Drum名称 = ''

                            SQL_Ex = "update Report_DrumN set 条码 = '" + BarCode + "',批号 = '" + SNCode + "',有效期 = '" + ExpData + "' where 更换时间 = '" + stimeB.ToString() + "' and 化学品名称 = 'Spin-D' and Drum名称 = 'DrumB'";

                            //SQL_Ex = "INSERT INTO Report_DrumN(更换时间,化学品名称,Drum名称,条码) Values('" + stime.ToString() + "','Spin-D','" + ObjectN + "','" + BarCode + "')";


                            AsyncExecuteNonQuery(SQL_Ex, CallbackAsyncExecuteNonQuery);
                            SpinD.Write("DB911.DBX292.0", false);

                        }
                        DrumB_lastStep = 0;
                    }

                    
                }

                await Task.Delay(1000);
            }

        }
        private static async Task GET_SC100()
        {
            Plc SC100 = new Plc(CpuType.S71500, "10.249.76.20", 0, 1);
            string BarCode;
            string SNCode;
            string ExpData;
            string SQL_Ex;
            int DrumA_lastStep;
            int DrumB_lastStep;
            int Lorry_lastStep;
            DateTime stimeA, stimeB, stimeC, stimeD;
            DrumA_lastStep = 0;
            DrumB_lastStep = 0;
            Lorry_lastStep = 0;
            stimeA = DateTime.Now;
            stimeB = DateTime.Now;
            stimeC = DateTime.Now;
            stimeD = DateTime.Now;
            while (true)
            {

                if (SC100.IsConnected == false)
                {
                    //Console.WriteLine("尝试连接SC100 PLC");
                    SC100.OpenAsync().Wait(2000);
                }

                if (SC100.IsConnected == true)
                {
                    var DrumA_Step = SC100.Read("DB1105.DBW0");
                    var DrumB_Step = SC100.Read("DB1205.DBW0");
                    if (DrumA_Step.ToString() == "3" && DrumA_lastStep == 0)
                    {
                        stimeA = DateTime.Now;
                        SQL_Ex = "exec Int_Drum '" + stimeA.ToString() + "','SC-100','DrumA'";
                        AsyncExecuteNonQuery(SQL_Ex, CallbackAsyncExecuteNonQuery);
                        DrumA_lastStep = 2;
                    }
                    if (DrumA_Step.ToString()  == "5" && DrumA_lastStep == 2)
                    {
                        var WriteBit = SC100.Read("DB911.DBX292.0");
                        if (WriteBit.ToString() == "True")
                        {

                            var m_BarCode = SC100.ReadBytes(DataType.DataBlock, 911, 324, 40);
                            var m_SNCode = SC100.ReadBytes(DataType.DataBlock, 911, 364, 40);
                            var m_ExpData = SC100.ReadBytes(DataType.DataBlock, 911, 404, 40);

                            BarCode = System.Text.ASCIIEncoding.ASCII.GetString(m_BarCode);
                            SNCode = System.Text.ASCIIEncoding.ASCII.GetString(m_SNCode);
                            ExpData = System.Text.ASCIIEncoding.ASCII.GetString(m_ExpData);

                            // update Report_DrumN set 条码 = '',批号 = '',有效期 = '' where 更换时间 = '' and 化学品名称 = '' and Drum名称 = ''

                            SQL_Ex = "update Report_DrumN set 条码 = '" + BarCode + "',批号 = '" + SNCode + "',有效期 = '" + ExpData + "' where 更换时间 = '" + stimeA.ToString() + "' and 化学品名称 = 'SC-100' and Drum名称 = 'DrumA'";

                            //SQL_Ex = "INSERT INTO Report_DrumN(更换时间,化学品名称,Drum名称,条码) Values('" + stime.ToString() + "','SC-100','" + ObjectN + "','" + BarCode + "')";


                            AsyncExecuteNonQuery(SQL_Ex, CallbackAsyncExecuteNonQuery);
                            SC100.Write("DB911.DBX292.0", false);

                        }
                        DrumA_lastStep = 0;
                    }

                    if (DrumB_Step.ToString() == "3" && DrumB_lastStep == 0)
                    {
                        stimeB = DateTime.Now;
                        SQL_Ex = "exec Int_Drum '" + stimeB.ToString() + "','SC-100','DrumB'";
                        AsyncExecuteNonQuery(SQL_Ex, CallbackAsyncExecuteNonQuery);
                        DrumB_lastStep = 2;
                    }
                    if (DrumB_Step.ToString()  == "5" && DrumB_lastStep == 2)
                    {
                        var WriteBit = SC100.Read("DB911.DBX292.0");
                        if (WriteBit.ToString() == "True")
                        {

                            var m_BarCode = SC100.ReadBytes(DataType.DataBlock, 911, 324, 40);
                            var m_SNCode = SC100.ReadBytes(DataType.DataBlock, 911, 364, 40);
                            var m_ExpData = SC100.ReadBytes(DataType.DataBlock, 911, 404, 40);

                            BarCode = System.Text.ASCIIEncoding.ASCII.GetString(m_BarCode);
                            SNCode = System.Text.ASCIIEncoding.ASCII.GetString(m_SNCode);
                            ExpData = System.Text.ASCIIEncoding.ASCII.GetString(m_ExpData);

                            // update Report_DrumN set 条码 = '',批号 = '',有效期 = '' where 更换时间 = '' and 化学品名称 = '' and Drum名称 = ''

                            SQL_Ex = "update Report_DrumN set 条码 = '" + BarCode + "',批号 = '" + SNCode + "',有效期 = '" + ExpData + "' where 更换时间 = '" + stimeB.ToString() + "' and 化学品名称 = 'SC-100' and Drum名称 = 'DrumB'";

                            //SQL_Ex = "INSERT INTO Report_DrumN(更换时间,化学品名称,Drum名称,条码) Values('" + stime.ToString() + "','SC-100','" + ObjectN + "','" + BarCode + "')";


                            AsyncExecuteNonQuery(SQL_Ex, CallbackAsyncExecuteNonQuery);
                            SC100.Write("DB911.DBX292.0", false);

                        }
                        DrumB_lastStep = 0;
                    }
                                       
                }

                await Task.Delay(1000);
            }

        }
        private static async Task GET_NH4OH()
        {
            Plc NH4OH = new Plc(CpuType.S71500, "10.249.76.22", 0, 1);
            string BarCode;
            string SNCode;
            string ExpData;
            string SQL_Ex;
            int DrumA_lastStep;
            int DrumB_lastStep;
            int Lorry_lastStep;
            DateTime stimeA, stimeB, stimeC, stimeD;
            DrumA_lastStep = 0;
            DrumB_lastStep = 0;
            Lorry_lastStep = 0;
            stimeA = DateTime.Now;
            stimeB = DateTime.Now;
            stimeC = DateTime.Now;
            stimeD = DateTime.Now;
            while (true)
            {

                if (NH4OH.IsConnected == false)
                {
                    //Console.WriteLine("尝试连接NH4OH PLC");
                    NH4OH.OpenAsync().Wait(2000);
                }

                if (NH4OH.IsConnected == true)
                {
                    var DrumA_Step = NH4OH.Read("DB1105.DBW0");
                    var DrumB_Step = NH4OH.Read("DB1205.DBW0");
                    if (DrumA_Step.ToString() == "3" && DrumA_lastStep == 0)
                    {
                        stimeA = DateTime.Now;
                        SQL_Ex = "exec Int_Drum '" + stimeA.ToString() + "','NH4OH','DrumA'";
                        AsyncExecuteNonQuery(SQL_Ex, CallbackAsyncExecuteNonQuery);
                        DrumA_lastStep = 2;
                    }
                    if (DrumA_Step.ToString()  == "5" && DrumA_lastStep == 2)
                    {
                        var WriteBit = NH4OH.Read("DB911.DBX292.0");
                        if (WriteBit.ToString() == "True")
                        {

                            var m_BarCode = NH4OH.ReadBytes(DataType.DataBlock, 911, 324, 40);
                            var m_SNCode = NH4OH.ReadBytes(DataType.DataBlock, 911, 364, 40);
                            var m_ExpData = NH4OH.ReadBytes(DataType.DataBlock, 911, 404, 40);

                            BarCode = System.Text.ASCIIEncoding.ASCII.GetString(m_BarCode);
                            SNCode = System.Text.ASCIIEncoding.ASCII.GetString(m_SNCode);
                            ExpData = System.Text.ASCIIEncoding.ASCII.GetString(m_ExpData);

                            // update Report_DrumN set 条码 = '',批号 = '',有效期 = '' where 更换时间 = '' and 化学品名称 = '' and Drum名称 = ''

                            SQL_Ex = "update Report_DrumN set 条码 = '" + BarCode + "',批号 = '" + SNCode + "',有效期 = '" + ExpData + "' where 更换时间 = '" + stimeA.ToString() + "' and 化学品名称 = 'NH4OH' and Drum名称 = 'DrumA'";

                            //SQL_Ex = "INSERT INTO Report_DrumN(更换时间,化学品名称,Drum名称,条码) Values('" + stime.ToString() + "','NH4OH','" + ObjectN + "','" + BarCode + "')";


                            AsyncExecuteNonQuery(SQL_Ex, CallbackAsyncExecuteNonQuery);
                            NH4OH.Write("DB911.DBX292.0", false);

                        }
                        DrumA_lastStep = 0;
                    }

                    if (DrumB_Step.ToString() == "3" && DrumB_lastStep == 0)
                    {
                        stimeB = DateTime.Now;
                        SQL_Ex = "exec Int_Drum '" + stimeB.ToString() + "','NH4OH','DrumB'";
                        AsyncExecuteNonQuery(SQL_Ex, CallbackAsyncExecuteNonQuery);
                        DrumB_lastStep = 2;
                    }
                    if (DrumB_Step.ToString()  == "5" && DrumB_lastStep == 2)
                    {
                        var WriteBit = NH4OH.Read("DB911.DBX292.0");
                        if (WriteBit.ToString() == "True")
                        {

                            var m_BarCode = NH4OH.ReadBytes(DataType.DataBlock, 911, 324, 40);
                            var m_SNCode = NH4OH.ReadBytes(DataType.DataBlock, 911, 364, 40);
                            var m_ExpData = NH4OH.ReadBytes(DataType.DataBlock, 911, 404, 40);

                            BarCode = System.Text.ASCIIEncoding.ASCII.GetString(m_BarCode);
                            SNCode = System.Text.ASCIIEncoding.ASCII.GetString(m_SNCode);
                            ExpData = System.Text.ASCIIEncoding.ASCII.GetString(m_ExpData);

                            // update Report_DrumN set 条码 = '',批号 = '',有效期 = '' where 更换时间 = '' and 化学品名称 = '' and Drum名称 = ''

                            SQL_Ex = "update Report_DrumN set 条码 = '" + BarCode + "',批号 = '" + SNCode + "',有效期 = '" + ExpData + "' where 更换时间 = '" + stimeB.ToString() + "' and 化学品名称 = 'NH4OH' and Drum名称 = 'DrumB'";

                            //SQL_Ex = "INSERT INTO Report_DrumN(更换时间,化学品名称,Drum名称,条码) Values('" + stime.ToString() + "','NH4OH','" + ObjectN + "','" + BarCode + "')";


                            AsyncExecuteNonQuery(SQL_Ex, CallbackAsyncExecuteNonQuery);
                            NH4OH.Write("DB911.DBX292.0", false);

                        }
                        DrumB_lastStep = 0;
                    }

                    var Lorry_Step = NH4OH.Read("DB5105.DBW0");
                    if (Lorry_Step.ToString() == "6" && Lorry_lastStep == 0)
                    {
                        stimeC = DateTime.Now;
                        //exec Int_Lorry '2022-06-26 15:37:20.000','NH4OH'
                        SQL_Ex = "exec Int_Lorry '" + stimeC.ToString() + "','NH4OH'";
                        AsyncExecuteNonQuery(SQL_Ex, CallbackAsyncExecuteNonQuery);
                        Lorry_lastStep = 3;
                    }
                    if (Lorry_Step.ToString() == "16" && Lorry_lastStep == 3)
                    {
                        stimeD = DateTime.Now;
                        SQL_Ex = "update Report_CCBN set 结束时间 = '" + stimeD.ToString() + "'";
                        var LWriteBit = (bool)NH4OH.Read("DB910.DBX292.0");
                        if (LWriteBit == true)
                        {
                            var m_BarCode = NH4OH.ReadBytes(DataType.DataBlock, 910, 324, 40);
                            var m_SNCode = NH4OH.ReadBytes(DataType.DataBlock, 910, 364, 40);
                            var m_ExpData = NH4OH.ReadBytes(DataType.DataBlock, 910, 404, 40);
                            BarCode = System.Text.ASCIIEncoding.ASCII.GetString(m_BarCode);
                            SNCode = System.Text.ASCIIEncoding.ASCII.GetString(m_SNCode);
                            ExpData = System.Text.ASCIIEncoding.ASCII.GetString(m_ExpData);
                            SQL_Ex = SQL_Ex + ",条码 = '" + BarCode + "',批号 = '" + SNCode + "',有效期 = '" + ExpData + "' where 化学品名称 = 'NH4OH' and 开始时间 = '" + stimeC.ToString() + "'";
                            NH4OH.Write("DB910.DBX292.0", false);

                        }
                        AsyncExecuteNonQuery(SQL_Ex, CallbackAsyncExecuteNonQuery);
                        Lorry_lastStep = 0;
                    }


                }

                await Task.Delay(1000);
            }

        }
        private static async Task GET_DEV_M()
        {
            Plc DEV_M = new Plc(CpuType.S71500, "10.249.76.24", 0, 1);
            string BarCode;
            string SNCode;
            string ExpData;
            string SQL_Ex;
            int DrumA_lastStep;
            int DrumB_lastStep;
            int Lorry_lastStep;
            DateTime stimeA, stimeB, stimeC, stimeD;
            DrumA_lastStep = 0;
            DrumB_lastStep = 0;
            Lorry_lastStep = 0;
            stimeA = DateTime.Now;
            stimeB = DateTime.Now;
            stimeC = DateTime.Now;
            stimeD = DateTime.Now;
            while (true)
            {

                if (DEV_M.IsConnected == false)
                {
                    //Console.WriteLine("尝试连接DEV_M PLC");
                    DEV_M.OpenAsync().Wait(2000);
                }

                if (DEV_M.IsConnected == true)
                {
                    var DrumA_Step = DEV_M.Read("DB1105.DBW0");
                    var DrumB_Step = DEV_M.Read("DB1205.DBW0");
                    if (DrumA_Step.ToString() == "3" && DrumA_lastStep == 0)
                    {
                        stimeA = DateTime.Now;
                        SQL_Ex = "exec Int_Drum '" + stimeA.ToString() + "','25%DEV','DrumA'";
                        AsyncExecuteNonQuery(SQL_Ex, CallbackAsyncExecuteNonQuery);
                        DrumA_lastStep = 2;
                    }
                    if (DrumA_Step.ToString()  == "5" && DrumA_lastStep == 2)
                    {
                        var WriteBit = DEV_M.Read("DB911.DBX292.0");
                        if (WriteBit.ToString() == "True")
                        {

                            var m_BarCode = DEV_M.ReadBytes(DataType.DataBlock, 911, 324, 40);
                            var m_SNCode = DEV_M.ReadBytes(DataType.DataBlock, 911, 364, 40);
                            var m_ExpData = DEV_M.ReadBytes(DataType.DataBlock, 911, 404, 40);

                            BarCode = System.Text.ASCIIEncoding.ASCII.GetString(m_BarCode);
                            SNCode = System.Text.ASCIIEncoding.ASCII.GetString(m_SNCode);
                            ExpData = System.Text.ASCIIEncoding.ASCII.GetString(m_ExpData);

                            // update Report_DrumN set 条码 = '',批号 = '',有效期 = '' where 更换时间 = '' and 化学品名称 = '' and Drum名称 = ''

                            SQL_Ex = "update Report_DrumN set 条码 = '" + BarCode + "',批号 = '" + SNCode + "',有效期 = '" + ExpData + "' where 更换时间 = '" + stimeA.ToString() + "' and 化学品名称 = '25%DEV' and Drum名称 = 'DrumA'";

                            //SQL_Ex = "INSERT INTO Report_DrumN(更换时间,化学品名称,Drum名称,条码) Values('" + stime.ToString() + "','25%DEV','" + ObjectN + "','" + BarCode + "')";


                            AsyncExecuteNonQuery(SQL_Ex, CallbackAsyncExecuteNonQuery);
                            DEV_M.Write("DB911.DBX292.0", false);

                        }
                        DrumA_lastStep = 0;
                    }

                    if (DrumB_Step.ToString() == "3" && DrumB_lastStep == 0)
                    {
                        stimeB = DateTime.Now;
                        SQL_Ex = "exec Int_Drum '" + stimeB.ToString() + "','25%DEV','DrumB'";
                        AsyncExecuteNonQuery(SQL_Ex, CallbackAsyncExecuteNonQuery);
                        DrumB_lastStep = 2;
                    }
                    if (DrumB_Step.ToString()  == "5" && DrumB_lastStep == 2)
                    {
                        var WriteBit = DEV_M.Read("DB911.DBX292.0");
                        if (WriteBit.ToString() == "True")
                        {

                            var m_BarCode = DEV_M.ReadBytes(DataType.DataBlock, 911, 324, 40);
                            var m_SNCode = DEV_M.ReadBytes(DataType.DataBlock, 911, 364, 40);
                            var m_ExpData = DEV_M.ReadBytes(DataType.DataBlock, 911, 404, 40);

                            BarCode = System.Text.ASCIIEncoding.ASCII.GetString(m_BarCode);
                            SNCode = System.Text.ASCIIEncoding.ASCII.GetString(m_SNCode);
                            ExpData = System.Text.ASCIIEncoding.ASCII.GetString(m_ExpData);

                            // update Report_DrumN set 条码 = '',批号 = '',有效期 = '' where 更换时间 = '' and 化学品名称 = '' and Drum名称 = ''

                            SQL_Ex = "update Report_DrumN set 条码 = '" + BarCode + "',批号 = '" + SNCode + "',有效期 = '" + ExpData + "' where 更换时间 = '" + stimeB.ToString() + "' and 化学品名称 = '25%DEV' and Drum名称 = 'DrumB'";

                            //SQL_Ex = "INSERT INTO Report_DrumN(更换时间,化学品名称,Drum名称,条码) Values('" + stime.ToString() + "','25%DEV','" + ObjectN + "','" + BarCode + "')";


                            AsyncExecuteNonQuery(SQL_Ex, CallbackAsyncExecuteNonQuery);
                            DEV_M.Write("DB911.DBX292.0", false);

                        }
                        DrumB_lastStep = 0;
                    }

                    var Lorry_Step = DEV_M.Read("DB5105.DBW0");
                    if (Lorry_Step.ToString() == "6" && Lorry_lastStep == 0)
                    {
                        stimeC = DateTime.Now;
                        //exec Int_Lorry '2022-06-26 15:37:20.000','25%DEV'
                        SQL_Ex = "exec Int_Lorry '" + stimeC.ToString() + "','25%DEV'";
                        AsyncExecuteNonQuery(SQL_Ex, CallbackAsyncExecuteNonQuery);
                        Lorry_lastStep = 3;
                    }
                    if (Lorry_Step.ToString() == "16" && Lorry_lastStep == 3)
                    {
                        stimeD = DateTime.Now;
                        SQL_Ex = "update Report_CCBN set 结束时间 = '" + stimeD.ToString() + "'";
                        var LWriteBit = (bool)DEV_M.Read("DB910.DBX292.0");
                        if (LWriteBit == true)
                        {
                            var m_BarCode = DEV_M.ReadBytes(DataType.DataBlock, 910, 324, 40);
                            var m_SNCode = DEV_M.ReadBytes(DataType.DataBlock, 910, 364, 40);
                            var m_ExpData = DEV_M.ReadBytes(DataType.DataBlock, 910, 404, 40);
                            BarCode = System.Text.ASCIIEncoding.ASCII.GetString(m_BarCode);
                            SNCode = System.Text.ASCIIEncoding.ASCII.GetString(m_SNCode);
                            ExpData = System.Text.ASCIIEncoding.ASCII.GetString(m_ExpData);
                            SQL_Ex = SQL_Ex + ",条码 = '" + BarCode + "',批号 = '" + SNCode + "',有效期 = '" + ExpData + "' where 化学品名称 = '25%DEV' and 开始时间 = '" + stimeC.ToString() + "'";
                            DEV_M.Write("DB910.DBX292.0", false);

                        }
                        AsyncExecuteNonQuery(SQL_Ex, CallbackAsyncExecuteNonQuery);
                        Lorry_lastStep = 0;
                    }


                }

                await Task.Delay(1000);
            }

        }
        private static async Task GET_PlanarClean()
        {
            Plc PlanarClean = new Plc(CpuType.S71500, "10.249.76.28", 0, 1);
            string BarCode;
            string SNCode;
            string ExpData;
            string SQL_Ex;
            int DrumA_lastStep;
            int DrumB_lastStep;
            int Lorry_lastStep;
            DateTime stimeA, stimeB, stimeC, stimeD;
            DrumA_lastStep = 0;
            DrumB_lastStep = 0;
            Lorry_lastStep = 0;
            stimeA = DateTime.Now;
            stimeB = DateTime.Now;
            stimeC = DateTime.Now;
            stimeD = DateTime.Now;
            while (true)
            {

                if (PlanarClean.IsConnected == false)
                {
                    //Console.WriteLine("尝试连接PlanarClean PLC");
                    PlanarClean.OpenAsync().Wait(2000);
                }

                if (PlanarClean.IsConnected == true)
                {
                    var DrumA_Step = PlanarClean.Read("DB1105.DBW0");
                    var DrumB_Step = PlanarClean.Read("DB1205.DBW0");
                    if (DrumA_Step.ToString() == "3" && DrumA_lastStep == 0)
                    {
                        stimeA = DateTime.Now;
                        SQL_Ex = "exec Int_Drum '" + stimeA.ToString() + "','PlanarClean','DrumA'";
                        AsyncExecuteNonQuery(SQL_Ex, CallbackAsyncExecuteNonQuery);
                        DrumA_lastStep = 2;
                    }
                    if (DrumA_Step.ToString()  == "5" && DrumA_lastStep == 2)
                    {
                        var WriteBit = PlanarClean.Read("DB911.DBX292.0");
                        if (WriteBit.ToString() == "True")
                        {

                            var m_BarCode = PlanarClean.ReadBytes(DataType.DataBlock, 911, 324, 40);
                            var m_SNCode = PlanarClean.ReadBytes(DataType.DataBlock, 911, 364, 40);
                            var m_ExpData = PlanarClean.ReadBytes(DataType.DataBlock, 911, 404, 40);

                            BarCode = System.Text.ASCIIEncoding.ASCII.GetString(m_BarCode);
                            SNCode = System.Text.ASCIIEncoding.ASCII.GetString(m_SNCode);
                            ExpData = System.Text.ASCIIEncoding.ASCII.GetString(m_ExpData);

                            // update Report_DrumN set 条码 = '',批号 = '',有效期 = '' where 更换时间 = '' and 化学品名称 = '' and Drum名称 = ''

                            SQL_Ex = "update Report_DrumN set 条码 = '" + BarCode + "',批号 = '" + SNCode + "',有效期 = '" + ExpData + "' where 更换时间 = '" + stimeA.ToString() + "' and 化学品名称 = 'PlanarClean' and Drum名称 = 'DrumA'";

                            //SQL_Ex = "INSERT INTO Report_DrumN(更换时间,化学品名称,Drum名称,条码) Values('" + stime.ToString() + "','PlanarClean','" + ObjectN + "','" + BarCode + "')";


                            AsyncExecuteNonQuery(SQL_Ex, CallbackAsyncExecuteNonQuery);
                            PlanarClean.Write("DB911.DBX292.0", false);

                        }
                        DrumA_lastStep = 0;
                    }

                    if (DrumB_Step.ToString() == "3" && DrumB_lastStep == 0)
                    {
                        stimeB = DateTime.Now;
                        SQL_Ex = "exec Int_Drum '" + stimeB.ToString() + "','PlanarClean','DrumB'";
                        AsyncExecuteNonQuery(SQL_Ex, CallbackAsyncExecuteNonQuery);
                        DrumB_lastStep = 2;
                    }
                    if (DrumB_Step.ToString()  == "5" && DrumB_lastStep == 2)
                    {
                        var WriteBit = PlanarClean.Read("DB911.DBX292.0");
                        if (WriteBit.ToString() == "True")
                        {

                            var m_BarCode = PlanarClean.ReadBytes(DataType.DataBlock, 911, 324, 40);
                            var m_SNCode = PlanarClean.ReadBytes(DataType.DataBlock, 911, 364, 40);
                            var m_ExpData = PlanarClean.ReadBytes(DataType.DataBlock, 911, 404, 40);

                            BarCode = System.Text.ASCIIEncoding.ASCII.GetString(m_BarCode);
                            SNCode = System.Text.ASCIIEncoding.ASCII.GetString(m_SNCode);
                            ExpData = System.Text.ASCIIEncoding.ASCII.GetString(m_ExpData);

                            // update Report_DrumN set 条码 = '',批号 = '',有效期 = '' where 更换时间 = '' and 化学品名称 = '' and Drum名称 = ''

                            SQL_Ex = "update Report_DrumN set 条码 = '" + BarCode + "',批号 = '" + SNCode + "',有效期 = '" + ExpData + "' where 更换时间 = '" + stimeB.ToString() + "' and 化学品名称 = 'PlanarClean' and Drum名称 = 'DrumB'";

                            //SQL_Ex = "INSERT INTO Report_DrumN(更换时间,化学品名称,Drum名称,条码) Values('" + stime.ToString() + "','PlanarClean','" + ObjectN + "','" + BarCode + "')";


                            AsyncExecuteNonQuery(SQL_Ex, CallbackAsyncExecuteNonQuery);
                            PlanarClean.Write("DB911.DBX292.0", false);

                        }
                        DrumB_lastStep = 0;
                    }
                                        
                }

                await Task.Delay(1000);
            }

        }
        private static async Task GET_HNO3A()
        {
            Plc HNO3A = new Plc(CpuType.S71500, "10.249.76.30", 0, 1);
            string BarCode;
            string SNCode;
            string ExpData;
            string SQL_Ex;
            int DrumA_lastStep;
            int DrumB_lastStep;
            int Lorry_lastStep;
            DateTime stimeA, stimeB, stimeC, stimeD;
            DrumA_lastStep = 0;
            DrumB_lastStep = 0;
            Lorry_lastStep = 0;
            stimeA = DateTime.Now;
            stimeB = DateTime.Now;
            stimeC = DateTime.Now;
            stimeD = DateTime.Now;
            while (true)
            {

                if (HNO3A.IsConnected == false)
                {
                    //Console.WriteLine("尝试连接HNO3A PLC");
                    HNO3A.OpenAsync().Wait(2000);
                }

                if (HNO3A.IsConnected == true)
                {
                    var DrumA_Step = HNO3A.Read("DB1105.DBW0");
                    var DrumB_Step = HNO3A.Read("DB1205.DBW0");
                    if (DrumA_Step.ToString() == "3" && DrumA_lastStep == 0)
                    {
                        stimeA = DateTime.Now;
                        SQL_Ex = "exec Int_Drum '" + stimeA.ToString() + "','HNO3A','DrumA'";
                        AsyncExecuteNonQuery(SQL_Ex, CallbackAsyncExecuteNonQuery);
                        DrumA_lastStep = 2;
                    }
                    if (DrumA_Step.ToString()  == "5" && DrumA_lastStep == 2)
                    {
                        var WriteBit = HNO3A.Read("DB911.DBX292.0");
                        if (WriteBit.ToString() == "True")
                        {

                            var m_BarCode = HNO3A.ReadBytes(DataType.DataBlock, 911, 324, 40);
                            var m_SNCode = HNO3A.ReadBytes(DataType.DataBlock, 911, 364, 40);
                            var m_ExpData = HNO3A.ReadBytes(DataType.DataBlock, 911, 404, 40);

                            BarCode = System.Text.ASCIIEncoding.ASCII.GetString(m_BarCode);
                            SNCode = System.Text.ASCIIEncoding.ASCII.GetString(m_SNCode);
                            ExpData = System.Text.ASCIIEncoding.ASCII.GetString(m_ExpData);

                            // update Report_DrumN set 条码 = '',批号 = '',有效期 = '' where 更换时间 = '' and 化学品名称 = '' and Drum名称 = ''

                            SQL_Ex = "update Report_DrumN set 条码 = '" + BarCode + "',批号 = '" + SNCode + "',有效期 = '" + ExpData + "' where 更换时间 = '" + stimeA.ToString() + "' and 化学品名称 = 'HNO3A' and Drum名称 = 'DrumA'";

                            //SQL_Ex = "INSERT INTO Report_DrumN(更换时间,化学品名称,Drum名称,条码) Values('" + stime.ToString() + "','HNO3A','" + ObjectN + "','" + BarCode + "')";


                            AsyncExecuteNonQuery(SQL_Ex, CallbackAsyncExecuteNonQuery);
                            HNO3A.Write("DB911.DBX292.0", false);

                        }
                        DrumA_lastStep = 0;
                    }

                    if (DrumB_Step.ToString() == "3" && DrumB_lastStep == 0)
                    {
                        stimeB = DateTime.Now;
                        SQL_Ex = "exec Int_Drum '" + stimeB.ToString() + "','HNO3A','DrumB'";
                        AsyncExecuteNonQuery(SQL_Ex, CallbackAsyncExecuteNonQuery);
                        DrumB_lastStep = 2;
                    }
                    if (DrumB_Step.ToString()  == "5" && DrumB_lastStep == 2)
                    {
                        var WriteBit = HNO3A.Read("DB911.DBX292.0");
                        if (WriteBit.ToString() == "True")
                        {

                            var m_BarCode = HNO3A.ReadBytes(DataType.DataBlock, 911, 324, 40);
                            var m_SNCode = HNO3A.ReadBytes(DataType.DataBlock, 911, 364, 40);
                            var m_ExpData = HNO3A.ReadBytes(DataType.DataBlock, 911, 404, 40);

                            BarCode = System.Text.ASCIIEncoding.ASCII.GetString(m_BarCode);
                            SNCode = System.Text.ASCIIEncoding.ASCII.GetString(m_SNCode);
                            ExpData = System.Text.ASCIIEncoding.ASCII.GetString(m_ExpData);

                            // update Report_DrumN set 条码 = '',批号 = '',有效期 = '' where 更换时间 = '' and 化学品名称 = '' and Drum名称 = ''

                            SQL_Ex = "update Report_DrumN set 条码 = '" + BarCode + "',批号 = '" + SNCode + "',有效期 = '" + ExpData + "' where 更换时间 = '" + stimeB.ToString() + "' and 化学品名称 = 'HNO3A' and Drum名称 = 'DrumB'";

                            //SQL_Ex = "INSERT INTO Report_DrumN(更换时间,化学品名称,Drum名称,条码) Values('" + stime.ToString() + "','HNO3A','" + ObjectN + "','" + BarCode + "')";


                            AsyncExecuteNonQuery(SQL_Ex, CallbackAsyncExecuteNonQuery);
                            HNO3A.Write("DB911.DBX292.0", false);

                        }
                        DrumB_lastStep = 0;
                    }
                                        
                }

                await Task.Delay(1000);
            }

        }
        private static async Task GET_HNO3B()
        {
            Plc HNO3B = new Plc(CpuType.S71500, "10.249.76.32", 0, 1);
            string BarCode;
            string SNCode;
            string ExpData;
            string SQL_Ex;
            int DrumA_lastStep;
            int DrumB_lastStep;
            int Lorry_lastStep;
            DateTime stimeA, stimeB, stimeC, stimeD;
            DrumA_lastStep = 0;
            DrumB_lastStep = 0;
            Lorry_lastStep = 0;
            stimeA = DateTime.Now;
            stimeB = DateTime.Now;
            stimeC = DateTime.Now;
            stimeD = DateTime.Now;
            while (true)
            {

                if (HNO3B.IsConnected == false)
                {
                    //Console.WriteLine("尝试连接HNO3B PLC");
                    HNO3B.OpenAsync().Wait(2000);
                }

                if (HNO3B.IsConnected == true)
                {
                    var DrumA_Step = HNO3B.Read("DB1105.DBW0");
                    var DrumB_Step = HNO3B.Read("DB1205.DBW0");
                    if (DrumA_Step.ToString() == "3" && DrumA_lastStep == 0)
                    {
                        stimeA = DateTime.Now;
                        SQL_Ex = "exec Int_Drum '" + stimeA.ToString() + "','HNO3B','DrumA'";
                        AsyncExecuteNonQuery(SQL_Ex, CallbackAsyncExecuteNonQuery);
                        DrumA_lastStep = 2;
                    }
                    if (DrumA_Step.ToString()  == "5" && DrumA_lastStep == 2)
                    {
                        var WriteBit = HNO3B.Read("DB911.DBX292.0");
                        if (WriteBit.ToString() == "True")
                        {

                            var m_BarCode = HNO3B.ReadBytes(DataType.DataBlock, 911, 324, 40);
                            var m_SNCode = HNO3B.ReadBytes(DataType.DataBlock, 911, 364, 40);
                            var m_ExpData = HNO3B.ReadBytes(DataType.DataBlock, 911, 404, 40);

                            BarCode = System.Text.ASCIIEncoding.ASCII.GetString(m_BarCode);
                            SNCode = System.Text.ASCIIEncoding.ASCII.GetString(m_SNCode);
                            ExpData = System.Text.ASCIIEncoding.ASCII.GetString(m_ExpData);

                            // update Report_DrumN set 条码 = '',批号 = '',有效期 = '' where 更换时间 = '' and 化学品名称 = '' and Drum名称 = ''

                            SQL_Ex = "update Report_DrumN set 条码 = '" + BarCode + "',批号 = '" + SNCode + "',有效期 = '" + ExpData + "' where 更换时间 = '" + stimeA.ToString() + "' and 化学品名称 = 'HNO3B' and Drum名称 = 'DrumA'";

                            //SQL_Ex = "INSERT INTO Report_DrumN(更换时间,化学品名称,Drum名称,条码) Values('" + stime.ToString() + "','HNO3B','" + ObjectN + "','" + BarCode + "')";


                            AsyncExecuteNonQuery(SQL_Ex, CallbackAsyncExecuteNonQuery);
                            HNO3B.Write("DB911.DBX292.0", false);

                        }
                        DrumA_lastStep = 0;
                    }

                    if (DrumB_Step.ToString() == "3" && DrumB_lastStep == 0)
                    {
                        stimeB = DateTime.Now;
                        SQL_Ex = "exec Int_Drum '" + stimeB.ToString() + "','HNO3B','DrumB'";
                        AsyncExecuteNonQuery(SQL_Ex, CallbackAsyncExecuteNonQuery);
                        DrumB_lastStep = 2;
                    }
                    if (DrumB_Step.ToString()  == "5" && DrumB_lastStep == 2)
                    {
                        var WriteBit = HNO3B.Read("DB911.DBX292.0");
                        if (WriteBit.ToString() == "True")
                        {

                            var m_BarCode = HNO3B.ReadBytes(DataType.DataBlock, 911, 324, 40);
                            var m_SNCode = HNO3B.ReadBytes(DataType.DataBlock, 911, 364, 40);
                            var m_ExpData = HNO3B.ReadBytes(DataType.DataBlock, 911, 404, 40);

                            BarCode = System.Text.ASCIIEncoding.ASCII.GetString(m_BarCode);
                            SNCode = System.Text.ASCIIEncoding.ASCII.GetString(m_SNCode);
                            ExpData = System.Text.ASCIIEncoding.ASCII.GetString(m_ExpData);

                            // update Report_DrumN set 条码 = '',批号 = '',有效期 = '' where 更换时间 = '' and 化学品名称 = '' and Drum名称 = ''

                            SQL_Ex = "update Report_DrumN set 条码 = '" + BarCode + "',批号 = '" + SNCode + "',有效期 = '" + ExpData + "' where 更换时间 = '" + stimeB.ToString() + "' and 化学品名称 = 'HNO3B' and Drum名称 = 'DrumB'";

                            //SQL_Ex = "INSERT INTO Report_DrumN(更换时间,化学品名称,Drum名称,条码) Values('" + stime.ToString() + "','HNO3B','" + ObjectN + "','" + BarCode + "')";


                            AsyncExecuteNonQuery(SQL_Ex, CallbackAsyncExecuteNonQuery);
                            HNO3B.Write("DB911.DBX292.0", false);

                        }
                        DrumB_lastStep = 0;
                    }

                }

                await Task.Delay(1000);
            }

        }
        private static async Task GET_H2O2A()
        {
            Plc H2O2A = new Plc(CpuType.S71500, "10.249.76.34", 0, 1);
            string BarCode;
            string SNCode;
            string ExpData;
            string SQL_Ex;
            int DrumA_lastStep;
            int DrumB_lastStep;
            int Lorry_lastStep;
            DateTime stimeA, stimeB, stimeC, stimeD;
            DrumA_lastStep = 0;
            DrumB_lastStep = 0;
            Lorry_lastStep = 0;
            stimeA = DateTime.Now;
            stimeB = DateTime.Now;
            stimeC = DateTime.Now;
            stimeD = DateTime.Now;
            while (true)
            {

                if (H2O2A.IsConnected == false)
                {
                    //Console.WriteLine("尝试连接H2O2A PLC");
                    H2O2A.OpenAsync().Wait(2000);
                }

                if (H2O2A.IsConnected == true)
                {
                    var DrumA_Step = H2O2A.Read("DB1105.DBW0");
                    var DrumB_Step = H2O2A.Read("DB1205.DBW0");
                    if (DrumA_Step.ToString() == "3" && DrumA_lastStep == 0)
                    {
                        stimeA = DateTime.Now;
                        SQL_Ex = "exec Int_Drum '" + stimeA.ToString() + "','H2O2A','DrumA'";
                        AsyncExecuteNonQuery(SQL_Ex, CallbackAsyncExecuteNonQuery);
                        DrumA_lastStep = 2;
                    }
                    if (DrumA_Step.ToString()  == "5" && DrumA_lastStep == 2)
                    {
                        var WriteBit = H2O2A.Read("DB911.DBX292.0");
                        if (WriteBit.ToString() == "True")
                        {

                            var m_BarCode = H2O2A.ReadBytes(DataType.DataBlock, 911, 324, 40);
                            var m_SNCode = H2O2A.ReadBytes(DataType.DataBlock, 911, 364, 40);
                            var m_ExpData = H2O2A.ReadBytes(DataType.DataBlock, 911, 404, 40);

                            BarCode = System.Text.ASCIIEncoding.ASCII.GetString(m_BarCode);
                            SNCode = System.Text.ASCIIEncoding.ASCII.GetString(m_SNCode);
                            ExpData = System.Text.ASCIIEncoding.ASCII.GetString(m_ExpData);

                            // update Report_DrumN set 条码 = '',批号 = '',有效期 = '' where 更换时间 = '' and 化学品名称 = '' and Drum名称 = ''

                            SQL_Ex = "update Report_DrumN set 条码 = '" + BarCode + "',批号 = '" + SNCode + "',有效期 = '" + ExpData + "' where 更换时间 = '" + stimeA.ToString() + "' and 化学品名称 = 'H2O2A' and Drum名称 = 'DrumA'";

                            //SQL_Ex = "INSERT INTO Report_DrumN(更换时间,化学品名称,Drum名称,条码) Values('" + stime.ToString() + "','H2O2A','" + ObjectN + "','" + BarCode + "')";


                            AsyncExecuteNonQuery(SQL_Ex, CallbackAsyncExecuteNonQuery);
                            H2O2A.Write("DB911.DBX292.0", false);

                        }
                        DrumA_lastStep = 0;
                    }

                    if (DrumB_Step.ToString() == "3" && DrumB_lastStep == 0)
                    {
                        stimeB = DateTime.Now;
                        SQL_Ex = "exec Int_Drum '" + stimeB.ToString() + "','H2O2A','DrumB'";
                        AsyncExecuteNonQuery(SQL_Ex, CallbackAsyncExecuteNonQuery);
                        DrumB_lastStep = 2;
                    }
                    if (DrumB_Step.ToString()  == "5" && DrumB_lastStep == 2)
                    {
                        var WriteBit = H2O2A.Read("DB911.DBX292.0");
                        if (WriteBit.ToString() == "True")
                        {

                            var m_BarCode = H2O2A.ReadBytes(DataType.DataBlock, 911, 324, 40);
                            var m_SNCode = H2O2A.ReadBytes(DataType.DataBlock, 911, 364, 40);
                            var m_ExpData = H2O2A.ReadBytes(DataType.DataBlock, 911, 404, 40);

                            BarCode = System.Text.ASCIIEncoding.ASCII.GetString(m_BarCode);
                            SNCode = System.Text.ASCIIEncoding.ASCII.GetString(m_SNCode);
                            ExpData = System.Text.ASCIIEncoding.ASCII.GetString(m_ExpData);

                            // update Report_DrumN set 条码 = '',批号 = '',有效期 = '' where 更换时间 = '' and 化学品名称 = '' and Drum名称 = ''

                            SQL_Ex = "update Report_DrumN set 条码 = '" + BarCode + "',批号 = '" + SNCode + "',有效期 = '" + ExpData + "' where 更换时间 = '" + stimeB.ToString() + "' and 化学品名称 = 'H2O2A' and Drum名称 = 'DrumB'";

                            //SQL_Ex = "INSERT INTO Report_DrumN(更换时间,化学品名称,Drum名称,条码) Values('" + stime.ToString() + "','H2O2A','" + ObjectN + "','" + BarCode + "')";


                            AsyncExecuteNonQuery(SQL_Ex, CallbackAsyncExecuteNonQuery);
                            H2O2A.Write("DB911.DBX292.0", false);

                        }
                        DrumB_lastStep = 0;
                    }

                    var Lorry_Step = H2O2A.Read("DB5105.DBW0");
                    if (Lorry_Step.ToString() == "6" && Lorry_lastStep == 0)
                    {
                        stimeC = DateTime.Now;
                        //exec Int_Lorry '2022-06-26 15:37:20.000','H2O2A'
                        SQL_Ex = "exec Int_Lorry '" + stimeC.ToString() + "','H2O2A'";
                        AsyncExecuteNonQuery(SQL_Ex, CallbackAsyncExecuteNonQuery);
                        Lorry_lastStep = 3;
                    }
                    if (Lorry_Step.ToString() == "16" && Lorry_lastStep == 3)
                    {
                        stimeD = DateTime.Now;
                        SQL_Ex = "update Report_CCBN set 结束时间 = '" + stimeD.ToString() + "'";
                        var LWriteBit = (bool)H2O2A.Read("DB910.DBX292.0");
                        if (LWriteBit == true)
                        {
                            var m_BarCode = H2O2A.ReadBytes(DataType.DataBlock, 910, 324, 40);
                            var m_SNCode = H2O2A.ReadBytes(DataType.DataBlock, 910, 364, 40);
                            var m_ExpData = H2O2A.ReadBytes(DataType.DataBlock, 910, 404, 40);
                            BarCode = System.Text.ASCIIEncoding.ASCII.GetString(m_BarCode);
                            SNCode = System.Text.ASCIIEncoding.ASCII.GetString(m_SNCode);
                            ExpData = System.Text.ASCIIEncoding.ASCII.GetString(m_ExpData);
                            SQL_Ex = SQL_Ex + ",条码 = '" + BarCode + "',批号 = '" + SNCode + "',有效期 = '" + ExpData + "' where 化学品名称 = 'H2O2A' and 开始时间 = '" + stimeC.ToString() + "'";
                            H2O2A.Write("DB910.DBX292.0", false);

                        }
                        AsyncExecuteNonQuery(SQL_Ex, CallbackAsyncExecuteNonQuery);
                        Lorry_lastStep = 0;
                    }


                }

                await Task.Delay(1000);
            }

        }
        private static async Task GET_H2O2B()
        {
            Plc H2O2B = new Plc(CpuType.S71500, "10.249.76.36", 0, 1);
            string BarCode;
            string SNCode;
            string ExpData;
            string SQL_Ex;
            int DrumA_lastStep;
            int DrumB_lastStep;
            int Lorry_lastStep;
            DateTime stimeA, stimeB, stimeC, stimeD;
            DrumA_lastStep = 0;
            DrumB_lastStep = 0;
            Lorry_lastStep = 0;
            stimeA = DateTime.Now;
            stimeB = DateTime.Now;
            stimeC = DateTime.Now;
            stimeD = DateTime.Now;
            while (true)
            {

                if (H2O2B.IsConnected == false)
                {
                    //Console.WriteLine("尝试连接H2O2B PLC");
                    H2O2B.OpenAsync().Wait(2000);
                }

                if (H2O2B.IsConnected == true)
                {
                    var DrumA_Step = H2O2B.Read("DB1105.DBW0");
                    var DrumB_Step = H2O2B.Read("DB1205.DBW0");
                    if (DrumA_Step.ToString() == "3" && DrumA_lastStep == 0)
                    {
                        stimeA = DateTime.Now;
                        SQL_Ex = "exec Int_Drum '" + stimeA.ToString() + "','H2O2B','DrumA'";
                        AsyncExecuteNonQuery(SQL_Ex, CallbackAsyncExecuteNonQuery);
                        DrumA_lastStep = 2;
                    }
                    if (DrumA_Step.ToString()  == "5" && DrumA_lastStep == 2)
                    {
                        var WriteBit = H2O2B.Read("DB911.DBX292.0");
                        if (WriteBit.ToString() == "True")
                        {

                            var m_BarCode = H2O2B.ReadBytes(DataType.DataBlock, 911, 324, 40);
                            var m_SNCode = H2O2B.ReadBytes(DataType.DataBlock, 911, 364, 40);
                            var m_ExpData = H2O2B.ReadBytes(DataType.DataBlock, 911, 404, 40);

                            BarCode = System.Text.ASCIIEncoding.ASCII.GetString(m_BarCode);
                            SNCode = System.Text.ASCIIEncoding.ASCII.GetString(m_SNCode);
                            ExpData = System.Text.ASCIIEncoding.ASCII.GetString(m_ExpData);

                            // update Report_DrumN set 条码 = '',批号 = '',有效期 = '' where 更换时间 = '' and 化学品名称 = '' and Drum名称 = ''

                            SQL_Ex = "update Report_DrumN set 条码 = '" + BarCode + "',批号 = '" + SNCode + "',有效期 = '" + ExpData + "' where 更换时间 = '" + stimeA.ToString() + "' and 化学品名称 = 'H2O2B' and Drum名称 = 'DrumA'";

                            //SQL_Ex = "INSERT INTO Report_DrumN(更换时间,化学品名称,Drum名称,条码) Values('" + stime.ToString() + "','H2O2B','" + ObjectN + "','" + BarCode + "')";


                            AsyncExecuteNonQuery(SQL_Ex, CallbackAsyncExecuteNonQuery);
                            H2O2B.Write("DB911.DBX292.0", false);

                        }
                        DrumA_lastStep = 0;
                    }

                    if (DrumB_Step.ToString() == "3" && DrumB_lastStep == 0)
                    {
                        stimeB = DateTime.Now;
                        SQL_Ex = "exec Int_Drum '" + stimeB.ToString() + "','H2O2B','DrumB'";
                        AsyncExecuteNonQuery(SQL_Ex, CallbackAsyncExecuteNonQuery);
                        DrumB_lastStep = 2;
                    }
                    if (DrumB_Step.ToString()  == "5" && DrumB_lastStep == 2)
                    {
                        var WriteBit = H2O2B.Read("DB911.DBX292.0");
                        if (WriteBit.ToString() == "True")
                        {

                            var m_BarCode = H2O2B.ReadBytes(DataType.DataBlock, 911, 324, 40);
                            var m_SNCode = H2O2B.ReadBytes(DataType.DataBlock, 911, 364, 40);
                            var m_ExpData = H2O2B.ReadBytes(DataType.DataBlock, 911, 404, 40);

                            BarCode = System.Text.ASCIIEncoding.ASCII.GetString(m_BarCode);
                            SNCode = System.Text.ASCIIEncoding.ASCII.GetString(m_SNCode);
                            ExpData = System.Text.ASCIIEncoding.ASCII.GetString(m_ExpData);

                            // update Report_DrumN set 条码 = '',批号 = '',有效期 = '' where 更换时间 = '' and 化学品名称 = '' and Drum名称 = ''

                            SQL_Ex = "update Report_DrumN set 条码 = '" + BarCode + "',批号 = '" + SNCode + "',有效期 = '" + ExpData + "' where 更换时间 = '" + stimeB.ToString() + "' and 化学品名称 = 'H2O2B' and Drum名称 = 'DrumB'";

                            //SQL_Ex = "INSERT INTO Report_DrumN(更换时间,化学品名称,Drum名称,条码) Values('" + stime.ToString() + "','H2O2B','" + ObjectN + "','" + BarCode + "')";


                            AsyncExecuteNonQuery(SQL_Ex, CallbackAsyncExecuteNonQuery);
                            H2O2B.Write("DB911.DBX292.0", false);

                        }
                        DrumB_lastStep = 0;
                    }

                    var Lorry_Step = H2O2B.Read("DB5105.DBW0");
                    if (Lorry_Step.ToString() == "6" && Lorry_lastStep == 0)
                    {
                        stimeC = DateTime.Now;
                        //exec Int_Lorry '2022-06-26 15:37:20.000','H2O2B'
                        SQL_Ex = "exec Int_Lorry '" + stimeC.ToString() + "','H2O2B'";
                        AsyncExecuteNonQuery(SQL_Ex, CallbackAsyncExecuteNonQuery);
                        Lorry_lastStep = 3;
                    }
                    if (Lorry_Step.ToString() == "16" && Lorry_lastStep == 3)
                    {
                        stimeD = DateTime.Now;
                        SQL_Ex = "update Report_CCBN set 结束时间 = '" + stimeD.ToString() + "'";
                        var LWriteBit = (bool)H2O2B.Read("DB910.DBX292.0");
                        if (LWriteBit == true)
                        {
                            var m_BarCode = H2O2B.ReadBytes(DataType.DataBlock, 910, 324, 40);
                            var m_SNCode = H2O2B.ReadBytes(DataType.DataBlock, 910, 364, 40);
                            var m_ExpData = H2O2B.ReadBytes(DataType.DataBlock, 910, 404, 40);
                            BarCode = System.Text.ASCIIEncoding.ASCII.GetString(m_BarCode);
                            SNCode = System.Text.ASCIIEncoding.ASCII.GetString(m_SNCode);
                            ExpData = System.Text.ASCIIEncoding.ASCII.GetString(m_ExpData);
                            SQL_Ex = SQL_Ex + ",条码 = '" + BarCode + "',批号 = '" + SNCode + "',有效期 = '" + ExpData + "' where 化学品名称 = 'H2O2B' and 开始时间 = '" + stimeC.ToString() + "'";
                            H2O2B.Write("DB910.DBX292.0", false);

                        }
                        AsyncExecuteNonQuery(SQL_Ex, CallbackAsyncExecuteNonQuery);
                        Lorry_lastStep = 0;
                    }


                }

                await Task.Delay(1000);
            }

        }
        private static async Task GET_IPAA()
        {
            Plc IPAA = new Plc(CpuType.S71500, "10.249.76.38", 0, 1);
            string BarCode;
            string SNCode;
            string ExpData;
            string SQL_Ex;
            int DrumA_lastStep;
            int DrumB_lastStep;
            int Lorry_lastStep;
            DateTime stimeA, stimeB, stimeC, stimeD;
            DrumA_lastStep = 0;
            DrumB_lastStep = 0;
            Lorry_lastStep = 0;
            stimeA = DateTime.Now;
            stimeB = DateTime.Now;
            stimeC = DateTime.Now;
            stimeD = DateTime.Now;
            while (true)
            {

                if (IPAA.IsConnected == false)
                {
                    //Console.WriteLine("尝试连接IPAA PLC");
                    IPAA.OpenAsync().Wait(2000);
                }

                if (IPAA.IsConnected == true)
                {
                    var DrumA_Step = IPAA.Read("DB1105.DBW0");
                    var DrumB_Step = IPAA.Read("DB1205.DBW0");
                    if (DrumA_Step.ToString() == "3" && DrumA_lastStep == 0)
                    {
                        stimeA = DateTime.Now;
                        SQL_Ex = "exec Int_Drum '" + stimeA.ToString() + "','IPAA','DrumA'";
                        AsyncExecuteNonQuery(SQL_Ex, CallbackAsyncExecuteNonQuery);
                        DrumA_lastStep = 2;
                    }
                    if (DrumA_Step.ToString()  == "5" && DrumA_lastStep == 2)
                    {
                        var WriteBit = IPAA.Read("DB911.DBX292.0");
                        if (WriteBit.ToString() == "True")
                        {

                            var m_BarCode = IPAA.ReadBytes(DataType.DataBlock, 911, 324, 40);
                            var m_SNCode = IPAA.ReadBytes(DataType.DataBlock, 911, 364, 40);
                            var m_ExpData = IPAA.ReadBytes(DataType.DataBlock, 911, 404, 40);

                            BarCode = System.Text.ASCIIEncoding.ASCII.GetString(m_BarCode);
                            SNCode = System.Text.ASCIIEncoding.ASCII.GetString(m_SNCode);
                            ExpData = System.Text.ASCIIEncoding.ASCII.GetString(m_ExpData);

                            // update Report_DrumN set 条码 = '',批号 = '',有效期 = '' where 更换时间 = '' and 化学品名称 = '' and Drum名称 = ''

                            SQL_Ex = "update Report_DrumN set 条码 = '" + BarCode + "',批号 = '" + SNCode + "',有效期 = '" + ExpData + "' where 更换时间 = '" + stimeA.ToString() + "' and 化学品名称 = 'IPAA' and Drum名称 = 'DrumA'";

                            //SQL_Ex = "INSERT INTO Report_DrumN(更换时间,化学品名称,Drum名称,条码) Values('" + stime.ToString() + "','IPAA','" + ObjectN + "','" + BarCode + "')";


                            AsyncExecuteNonQuery(SQL_Ex, CallbackAsyncExecuteNonQuery);
                            IPAA.Write("DB911.DBX292.0", false);

                        }
                        DrumA_lastStep = 0;
                    }

                    if (DrumB_Step.ToString() == "3" && DrumB_lastStep == 0)
                    {
                        stimeB = DateTime.Now;
                        SQL_Ex = "exec Int_Drum '" + stimeB.ToString() + "','IPAA','DrumB'";
                        AsyncExecuteNonQuery(SQL_Ex, CallbackAsyncExecuteNonQuery);
                        DrumB_lastStep = 2;
                    }
                    if (DrumB_Step.ToString()  == "5" && DrumB_lastStep == 2)
                    {
                        var WriteBit = IPAA.Read("DB911.DBX292.0");
                        if (WriteBit.ToString() == "True")
                        {

                            var m_BarCode = IPAA.ReadBytes(DataType.DataBlock, 911, 324, 40);
                            var m_SNCode = IPAA.ReadBytes(DataType.DataBlock, 911, 364, 40);
                            var m_ExpData = IPAA.ReadBytes(DataType.DataBlock, 911, 404, 40);

                            BarCode = System.Text.ASCIIEncoding.ASCII.GetString(m_BarCode);
                            SNCode = System.Text.ASCIIEncoding.ASCII.GetString(m_SNCode);
                            ExpData = System.Text.ASCIIEncoding.ASCII.GetString(m_ExpData);

                            // update Report_DrumN set 条码 = '',批号 = '',有效期 = '' where 更换时间 = '' and 化学品名称 = '' and Drum名称 = ''

                            SQL_Ex = "update Report_DrumN set 条码 = '" + BarCode + "',批号 = '" + SNCode + "',有效期 = '" + ExpData + "' where 更换时间 = '" + stimeB.ToString() + "' and 化学品名称 = 'IPAA' and Drum名称 = 'DrumB'";

                            //SQL_Ex = "INSERT INTO Report_DrumN(更换时间,化学品名称,Drum名称,条码) Values('" + stime.ToString() + "','IPAA','" + ObjectN + "','" + BarCode + "')";


                            AsyncExecuteNonQuery(SQL_Ex, CallbackAsyncExecuteNonQuery);
                            IPAA.Write("DB911.DBX292.0", false);

                        }
                        DrumB_lastStep = 0;
                    }
                                        
                }

                await Task.Delay(1000);
            }

        }
        private static async Task GET_IPAB()
        {
            Plc IPAB = new Plc(CpuType.S71500, "10.249.76.40", 0, 1);
            string BarCode;
            string SNCode;
            string ExpData;
            string SQL_Ex;
            int DrumA_lastStep;
            int DrumB_lastStep;
            int Lorry_lastStep;
            DateTime stimeA, stimeB, stimeC, stimeD;
            DrumA_lastStep = 0;
            DrumB_lastStep = 0;
            Lorry_lastStep = 0;
            stimeA = DateTime.Now;
            stimeB = DateTime.Now;
            stimeC = DateTime.Now;
            stimeD = DateTime.Now;
            while (true)
            {

                if (IPAB.IsConnected == false)
                {
                    //Console.WriteLine("尝试连接IPAB PLC");
                    IPAB.OpenAsync().Wait(2000);
                }

                if (IPAB.IsConnected == true)
                {
                    var DrumA_Step = IPAB.Read("DB1105.DBW0");
                    var DrumB_Step = IPAB.Read("DB1205.DBW0");
                    if (DrumA_Step.ToString() == "3" && DrumA_lastStep == 0)
                    {
                        stimeA = DateTime.Now;
                        SQL_Ex = "exec Int_Drum '" + stimeA.ToString() + "','IPAB','DrumA'";
                        AsyncExecuteNonQuery(SQL_Ex, CallbackAsyncExecuteNonQuery);
                        DrumA_lastStep = 2;
                    }
                    if (DrumA_Step.ToString()  == "5" && DrumA_lastStep == 2)
                    {
                        var WriteBit = IPAB.Read("DB911.DBX292.0");
                        if (WriteBit.ToString() == "True")
                        {

                            var m_BarCode = IPAB.ReadBytes(DataType.DataBlock, 911, 324, 40);
                            var m_SNCode = IPAB.ReadBytes(DataType.DataBlock, 911, 364, 40);
                            var m_ExpData = IPAB.ReadBytes(DataType.DataBlock, 911, 404, 40);

                            BarCode = System.Text.ASCIIEncoding.ASCII.GetString(m_BarCode);
                            SNCode = System.Text.ASCIIEncoding.ASCII.GetString(m_SNCode);
                            ExpData = System.Text.ASCIIEncoding.ASCII.GetString(m_ExpData);

                            // update Report_DrumN set 条码 = '',批号 = '',有效期 = '' where 更换时间 = '' and 化学品名称 = '' and Drum名称 = ''

                            SQL_Ex = "update Report_DrumN set 条码 = '" + BarCode + "',批号 = '" + SNCode + "',有效期 = '" + ExpData + "' where 更换时间 = '" + stimeA.ToString() + "' and 化学品名称 = 'IPAB' and Drum名称 = 'DrumA'";

                            //SQL_Ex = "INSERT INTO Report_DrumN(更换时间,化学品名称,Drum名称,条码) Values('" + stime.ToString() + "','IPAB','" + ObjectN + "','" + BarCode + "')";


                            AsyncExecuteNonQuery(SQL_Ex, CallbackAsyncExecuteNonQuery);
                            IPAB.Write("DB911.DBX292.0", false);

                        }
                        DrumA_lastStep = 0;
                    }

                    if (DrumB_Step.ToString() == "3" && DrumB_lastStep == 0)
                    {
                        stimeB = DateTime.Now;
                        SQL_Ex = "exec Int_Drum '" + stimeB.ToString() + "','IPAB','DrumB'";
                        AsyncExecuteNonQuery(SQL_Ex, CallbackAsyncExecuteNonQuery);
                        DrumB_lastStep = 2;
                    }
                    if (DrumB_Step.ToString()  == "5" && DrumB_lastStep == 2)
                    {
                        var WriteBit = IPAB.Read("DB911.DBX292.0");
                        if (WriteBit.ToString() == "True")
                        {

                            var m_BarCode = IPAB.ReadBytes(DataType.DataBlock, 911, 324, 40);
                            var m_SNCode = IPAB.ReadBytes(DataType.DataBlock, 911, 364, 40);
                            var m_ExpData = IPAB.ReadBytes(DataType.DataBlock, 911, 404, 40);

                            BarCode = System.Text.ASCIIEncoding.ASCII.GetString(m_BarCode);
                            SNCode = System.Text.ASCIIEncoding.ASCII.GetString(m_SNCode);
                            ExpData = System.Text.ASCIIEncoding.ASCII.GetString(m_ExpData);

                            // update Report_DrumN set 条码 = '',批号 = '',有效期 = '' where 更换时间 = '' and 化学品名称 = '' and Drum名称 = ''

                            SQL_Ex = "update Report_DrumN set 条码 = '" + BarCode + "',批号 = '" + SNCode + "',有效期 = '" + ExpData + "' where 更换时间 = '" + stimeB.ToString() + "' and 化学品名称 = 'IPAB' and Drum名称 = 'DrumB'";

                            //SQL_Ex = "INSERT INTO Report_DrumN(更换时间,化学品名称,Drum名称,条码) Values('" + stime.ToString() + "','IPAB','" + ObjectN + "','" + BarCode + "')";


                            AsyncExecuteNonQuery(SQL_Ex, CallbackAsyncExecuteNonQuery);
                            IPAB.Write("DB911.DBX292.0", false);

                        }
                        DrumB_lastStep = 0;
                    }                                        
                }

                await Task.Delay(1000);
            }

        }
        private static async Task GET_OK73A()
        {
            Plc OK73A = new Plc(CpuType.S71500, "10.249.76.42", 0, 1);
            string BarCode;
            string SNCode;
            string ExpData;
            string SQL_Ex;
            int DrumA_lastStep;
            int DrumB_lastStep;
            int Lorry_lastStep;
            DateTime stimeA, stimeB, stimeC, stimeD;
            DrumA_lastStep = 0;
            DrumB_lastStep = 0;
            Lorry_lastStep = 0;
            stimeA = DateTime.Now;
            stimeB = DateTime.Now;
            stimeC = DateTime.Now;
            stimeD = DateTime.Now;
            while (true)
            {

                if (OK73A.IsConnected == false)
                {
                    //Console.WriteLine("尝试连接OK73A PLC");
                    OK73A.OpenAsync().Wait(2000);
                }

                if (OK73A.IsConnected == true)
                {
                    var DrumA_Step = OK73A.Read("DB1105.DBW0");
                    var DrumB_Step = OK73A.Read("DB1205.DBW0");
                    if (DrumA_Step.ToString() == "3" && DrumA_lastStep == 0)
                    {
                        stimeA = DateTime.Now;
                        SQL_Ex = "exec Int_Drum '" + stimeA.ToString() + "','OK73A','DrumA'";
                        AsyncExecuteNonQuery(SQL_Ex, CallbackAsyncExecuteNonQuery);
                        DrumA_lastStep = 2;
                    }
                    if (DrumA_Step.ToString()  == "5" && DrumA_lastStep == 2)
                    {
                        var WriteBit = OK73A.Read("DB911.DBX292.0");
                        if (WriteBit.ToString() == "True")
                        {

                            var m_BarCode = OK73A.ReadBytes(DataType.DataBlock, 911, 324, 40);
                            var m_SNCode = OK73A.ReadBytes(DataType.DataBlock, 911, 364, 40);
                            var m_ExpData = OK73A.ReadBytes(DataType.DataBlock, 911, 404, 40);

                            BarCode = System.Text.ASCIIEncoding.ASCII.GetString(m_BarCode);
                            SNCode = System.Text.ASCIIEncoding.ASCII.GetString(m_SNCode);
                            ExpData = System.Text.ASCIIEncoding.ASCII.GetString(m_ExpData);

                            // update Report_DrumN set 条码 = '',批号 = '',有效期 = '' where 更换时间 = '' and 化学品名称 = '' and Drum名称 = ''

                            SQL_Ex = "update Report_DrumN set 条码 = '" + BarCode + "',批号 = '" + SNCode + "',有效期 = '" + ExpData + "' where 更换时间 = '" + stimeA.ToString() + "' and 化学品名称 = 'OK73A' and Drum名称 = 'DrumA'";

                            //SQL_Ex = "INSERT INTO Report_DrumN(更换时间,化学品名称,Drum名称,条码) Values('" + stime.ToString() + "','OK73A','" + ObjectN + "','" + BarCode + "')";


                            AsyncExecuteNonQuery(SQL_Ex, CallbackAsyncExecuteNonQuery);
                            OK73A.Write("DB911.DBX292.0", false);

                        }
                        DrumA_lastStep = 0;
                    }

                    if (DrumB_Step.ToString() == "3" && DrumB_lastStep == 0)
                    {
                        stimeB = DateTime.Now;
                        SQL_Ex = "exec Int_Drum '" + stimeB.ToString() + "','OK73A','DrumB'";
                        AsyncExecuteNonQuery(SQL_Ex, CallbackAsyncExecuteNonQuery);
                        DrumB_lastStep = 2;
                    }
                    if (DrumB_Step.ToString()  == "5" && DrumB_lastStep == 2)
                    {
                        var WriteBit = OK73A.Read("DB911.DBX292.0");
                        if (WriteBit.ToString() == "True")
                        {

                            var m_BarCode = OK73A.ReadBytes(DataType.DataBlock, 911, 324, 40);
                            var m_SNCode = OK73A.ReadBytes(DataType.DataBlock, 911, 364, 40);
                            var m_ExpData = OK73A.ReadBytes(DataType.DataBlock, 911, 404, 40);

                            BarCode = System.Text.ASCIIEncoding.ASCII.GetString(m_BarCode);
                            SNCode = System.Text.ASCIIEncoding.ASCII.GetString(m_SNCode);
                            ExpData = System.Text.ASCIIEncoding.ASCII.GetString(m_ExpData);

                            // update Report_DrumN set 条码 = '',批号 = '',有效期 = '' where 更换时间 = '' and 化学品名称 = '' and Drum名称 = ''

                            SQL_Ex = "update Report_DrumN set 条码 = '" + BarCode + "',批号 = '" + SNCode + "',有效期 = '" + ExpData + "' where 更换时间 = '" + stimeB.ToString() + "' and 化学品名称 = 'OK73A' and Drum名称 = 'DrumB'";

                            //SQL_Ex = "INSERT INTO Report_DrumN(更换时间,化学品名称,Drum名称,条码) Values('" + stime.ToString() + "','OK73A','" + ObjectN + "','" + BarCode + "')";


                            AsyncExecuteNonQuery(SQL_Ex, CallbackAsyncExecuteNonQuery);
                            OK73A.Write("DB911.DBX292.0", false);

                        }
                        DrumB_lastStep = 0;
                    }

                    var Lorry_Step = OK73A.Read("DB5105.DBW0");
                    if (Lorry_Step.ToString() == "6" && Lorry_lastStep == 0)
                    {
                        stimeC = DateTime.Now;
                        //exec Int_Lorry '2022-06-26 15:37:20.000','OK73A'
                        SQL_Ex = "exec Int_Lorry '" + stimeC.ToString() + "','OK73A')";
                        AsyncExecuteNonQuery(SQL_Ex, CallbackAsyncExecuteNonQuery);
                        Lorry_lastStep = 3;
                    }
                    if (Lorry_Step.ToString() == "16" && Lorry_lastStep == 3)
                    {
                        stimeD = DateTime.Now;
                        SQL_Ex = "update Report_CCBN set 结束时间 = '" + stimeD.ToString() + "'";
                        var LWriteBit = (bool)OK73A.Read("DB910.DBX292.0");
                        if (LWriteBit == true)
                        {
                            var m_BarCode = OK73A.ReadBytes(DataType.DataBlock, 910, 324, 40);
                            var m_SNCode = OK73A.ReadBytes(DataType.DataBlock, 910, 364, 40);
                            var m_ExpData = OK73A.ReadBytes(DataType.DataBlock, 910, 404, 40);
                            BarCode = System.Text.ASCIIEncoding.ASCII.GetString(m_BarCode);
                            SNCode = System.Text.ASCIIEncoding.ASCII.GetString(m_SNCode);
                            ExpData = System.Text.ASCIIEncoding.ASCII.GetString(m_ExpData);
                            SQL_Ex = SQL_Ex + ",条码 = '" + BarCode + "',批号 = '" + SNCode + "',有效期 = '" + ExpData + "' where 化学品名称 = 'OK73A' and 开始时间 = '" + stimeC.ToString() + "'";
                            OK73A.Write("DB910.DBX292.0", false);

                        }
                        AsyncExecuteNonQuery(SQL_Ex, CallbackAsyncExecuteNonQuery);
                        Lorry_lastStep = 0;
                    }


                }

                await Task.Delay(1000);
            }

        }
        private static async Task GET_OK73B()
        {
            Plc OK73B = new Plc(CpuType.S71500, "10.249.76.44", 0, 1);
            string BarCode;
            string SNCode;
            string ExpData;
            string SQL_Ex;
            int DrumA_lastStep;
            int DrumB_lastStep;
            int Lorry_lastStep;
            DateTime stimeA, stimeB, stimeC, stimeD;
            DrumA_lastStep = 0;
            DrumB_lastStep = 0;
            Lorry_lastStep = 0;
            stimeA = DateTime.Now;
            stimeB = DateTime.Now;
            stimeC = DateTime.Now;
            stimeD = DateTime.Now;
            while (true)
            {

                if (OK73B.IsConnected == false)
                {
                    //Console.WriteLine("尝试连接OK73B PLC");
                    OK73B.OpenAsync().Wait(2000);
                }

                if (OK73B.IsConnected == true)
                {
                    var DrumA_Step = OK73B.Read("DB1105.DBW0");
                    var DrumB_Step = OK73B.Read("DB1205.DBW0");
                    if (DrumA_Step.ToString() == "3" && DrumA_lastStep == 0)
                    {
                        stimeA = DateTime.Now;
                        SQL_Ex = "exec Int_Drum '" + stimeA.ToString() + "','OK73B','DrumA'";
                        AsyncExecuteNonQuery(SQL_Ex, CallbackAsyncExecuteNonQuery);
                        DrumA_lastStep = 2;
                    }
                    if (DrumA_Step.ToString()  == "5" && DrumA_lastStep == 2)
                    {
                        var WriteBit = OK73B.Read("DB911.DBX292.0");
                        if (WriteBit.ToString() == "True")
                        {

                            var m_BarCode = OK73B.ReadBytes(DataType.DataBlock, 911, 324, 40);
                            var m_SNCode = OK73B.ReadBytes(DataType.DataBlock, 911, 364, 40);
                            var m_ExpData = OK73B.ReadBytes(DataType.DataBlock, 911, 404, 40);

                            BarCode = System.Text.ASCIIEncoding.ASCII.GetString(m_BarCode);
                            SNCode = System.Text.ASCIIEncoding.ASCII.GetString(m_SNCode);
                            ExpData = System.Text.ASCIIEncoding.ASCII.GetString(m_ExpData);

                            // update Report_DrumN set 条码 = '',批号 = '',有效期 = '' where 更换时间 = '' and 化学品名称 = '' and Drum名称 = ''

                            SQL_Ex = "update Report_DrumN set 条码 = '" + BarCode + "',批号 = '" + SNCode + "',有效期 = '" + ExpData + "' where 更换时间 = '" + stimeA.ToString() + "' and 化学品名称 = 'OK73B' and Drum名称 = 'DrumA'";

                            //SQL_Ex = "INSERT INTO Report_DrumN(更换时间,化学品名称,Drum名称,条码) Values('" + stime.ToString() + "','OK73B','" + ObjectN + "','" + BarCode + "')";


                            AsyncExecuteNonQuery(SQL_Ex, CallbackAsyncExecuteNonQuery);
                            OK73B.Write("DB911.DBX292.0", false);

                        }
                        DrumA_lastStep = 0;
                    }

                    if (DrumB_Step.ToString() == "3" && DrumB_lastStep == 0)
                    {
                        stimeB = DateTime.Now;
                        SQL_Ex = "exec Int_Drum '" + stimeB.ToString() + "','OK73B','DrumB'";
                        AsyncExecuteNonQuery(SQL_Ex, CallbackAsyncExecuteNonQuery);
                        DrumB_lastStep = 2;
                    }
                    if (DrumB_Step.ToString()  == "5" && DrumB_lastStep == 2)
                    {
                        var WriteBit = OK73B.Read("DB911.DBX292.0");
                        if (WriteBit.ToString() == "True")
                        {

                            var m_BarCode = OK73B.ReadBytes(DataType.DataBlock, 911, 324, 40);
                            var m_SNCode = OK73B.ReadBytes(DataType.DataBlock, 911, 364, 40);
                            var m_ExpData = OK73B.ReadBytes(DataType.DataBlock, 911, 404, 40);

                            BarCode = System.Text.ASCIIEncoding.ASCII.GetString(m_BarCode);
                            SNCode = System.Text.ASCIIEncoding.ASCII.GetString(m_SNCode);
                            ExpData = System.Text.ASCIIEncoding.ASCII.GetString(m_ExpData);

                            // update Report_DrumN set 条码 = '',批号 = '',有效期 = '' where 更换时间 = '' and 化学品名称 = '' and Drum名称 = ''

                            SQL_Ex = "update Report_DrumN set 条码 = '" + BarCode + "',批号 = '" + SNCode + "',有效期 = '" + ExpData + "' where 更换时间 = '" + stimeB.ToString() + "' and 化学品名称 = 'OK73B' and Drum名称 = 'DrumB'";

                            //SQL_Ex = "INSERT INTO Report_DrumN(更换时间,化学品名称,Drum名称,条码) Values('" + stime.ToString() + "','OK73B','" + ObjectN + "','" + BarCode + "')";


                            AsyncExecuteNonQuery(SQL_Ex, CallbackAsyncExecuteNonQuery);
                            OK73B.Write("DB911.DBX292.0", false);

                        }
                        DrumB_lastStep = 0;
                    }

                    var Lorry_Step = OK73B.Read("DB5105.DBW0");
                    if (Lorry_Step.ToString() == "6" && Lorry_lastStep == 0)
                    {
                        stimeC = DateTime.Now;
                        //exec Int_Lorry '2022-06-26 15:37:20.000','OK73B'
                        SQL_Ex = "exec Int_Lorry '" + stimeC.ToString() + "','OK73B')";
                        AsyncExecuteNonQuery(SQL_Ex, CallbackAsyncExecuteNonQuery);
                        Lorry_lastStep = 3;
                    }
                    if (Lorry_Step.ToString() == "16" && Lorry_lastStep == 3)
                    {
                        stimeD = DateTime.Now;
                        SQL_Ex = "update Report_CCBN set 结束时间 = '" + stimeD.ToString() + "'";
                        var LWriteBit = (bool)OK73B.Read("DB910.DBX292.0");
                        if (LWriteBit == true)
                        {
                            var m_BarCode = OK73B.ReadBytes(DataType.DataBlock, 910, 324, 40);
                            var m_SNCode = OK73B.ReadBytes(DataType.DataBlock, 910, 364, 40);
                            var m_ExpData = OK73B.ReadBytes(DataType.DataBlock, 910, 404, 40);
                            BarCode = System.Text.ASCIIEncoding.ASCII.GetString(m_BarCode);
                            SNCode = System.Text.ASCIIEncoding.ASCII.GetString(m_SNCode);
                            ExpData = System.Text.ASCIIEncoding.ASCII.GetString(m_ExpData);
                            SQL_Ex = SQL_Ex + ",条码 = '" + BarCode + "',批号 = '" + SNCode + "',有效期 = '" + ExpData + "' where 化学品名称 = 'OK73B' and 开始时间 = '" + stimeC.ToString() + "'";
                            OK73B.Write("DB910.DBX292.0", false);

                        }
                        AsyncExecuteNonQuery(SQL_Ex, CallbackAsyncExecuteNonQuery);
                        Lorry_lastStep = 0;
                    }


                }

                await Task.Delay(1000);
            }

        }
        private static async Task GET_SYS9055A()
        {
            Plc SYS9055A = new Plc(CpuType.S71500, "10.249.76.46", 0, 1);
            string BarCode;
            string SNCode;
            string ExpData;
            string SQL_Ex;
            int DrumA_lastStep;
            int DrumB_lastStep;
            int Lorry_lastStep;
            DateTime stimeA, stimeB, stimeC, stimeD;
            DrumA_lastStep = 0;
            DrumB_lastStep = 0;
            Lorry_lastStep = 0;
            stimeA = DateTime.Now;
            stimeB = DateTime.Now;
            stimeC = DateTime.Now;
            stimeD = DateTime.Now;
            while (true)
            {

                if (SYS9055A.IsConnected == false)
                {
                    //Console.WriteLine("尝试连接SYS9055A PLC");
                    SYS9055A.OpenAsync().Wait(2000);
                }

                if (SYS9055A.IsConnected == true)
                {
                    var DrumA_Step = SYS9055A.Read("DB1105.DBW0");
                    var DrumB_Step = SYS9055A.Read("DB1205.DBW0");
                    if (DrumA_Step.ToString() == "3" && DrumA_lastStep == 0)
                    {
                        stimeA = DateTime.Now;
                        SQL_Ex = "exec Int_Drum '" + stimeA.ToString() + "','SYS9055A','DrumA'";
                        AsyncExecuteNonQuery(SQL_Ex, CallbackAsyncExecuteNonQuery);
                        DrumA_lastStep = 2;
                    }
                    if (DrumA_Step.ToString()  == "5" && DrumA_lastStep == 2)
                    {
                        var WriteBit = SYS9055A.Read("DB911.DBX292.0");
                        if (WriteBit.ToString() == "True")
                        {

                            var m_BarCode = SYS9055A.ReadBytes(DataType.DataBlock, 911, 324, 40);
                            var m_SNCode = SYS9055A.ReadBytes(DataType.DataBlock, 911, 364, 40);
                            var m_ExpData = SYS9055A.ReadBytes(DataType.DataBlock, 911, 404, 40);

                            BarCode = System.Text.ASCIIEncoding.ASCII.GetString(m_BarCode);
                            SNCode = System.Text.ASCIIEncoding.ASCII.GetString(m_SNCode);
                            ExpData = System.Text.ASCIIEncoding.ASCII.GetString(m_ExpData);

                            // update Report_DrumN set 条码 = '',批号 = '',有效期 = '' where 更换时间 = '' and 化学品名称 = '' and Drum名称 = ''

                            SQL_Ex = "update Report_DrumN set 条码 = '" + BarCode + "',批号 = '" + SNCode + "',有效期 = '" + ExpData + "' where 更换时间 = '" + stimeA.ToString() + "' and 化学品名称 = 'SYS9055A' and Drum名称 = 'DrumA'";

                            //SQL_Ex = "INSERT INTO Report_DrumN(更换时间,化学品名称,Drum名称,条码) Values('" + stime.ToString() + "','SYS9055A','" + ObjectN + "','" + BarCode + "')";


                            AsyncExecuteNonQuery(SQL_Ex, CallbackAsyncExecuteNonQuery);
                            SYS9055A.Write("DB911.DBX292.0", false);

                        }
                        DrumA_lastStep = 0;
                    }

                    if (DrumB_Step.ToString() == "3" && DrumB_lastStep == 0)
                    {
                        stimeB = DateTime.Now;
                        SQL_Ex = "exec Int_Drum '" + stimeB.ToString() + "','SYS9055A','DrumB'";
                        AsyncExecuteNonQuery(SQL_Ex, CallbackAsyncExecuteNonQuery);
                        DrumB_lastStep = 2;
                    }
                    if (DrumB_Step.ToString()  == "5" && DrumB_lastStep == 2)
                    {
                        var WriteBit = SYS9055A.Read("DB911.DBX292.0");
                        if (WriteBit.ToString() == "True")
                        {

                            var m_BarCode = SYS9055A.ReadBytes(DataType.DataBlock, 911, 324, 40);
                            var m_SNCode = SYS9055A.ReadBytes(DataType.DataBlock, 911, 364, 40);
                            var m_ExpData = SYS9055A.ReadBytes(DataType.DataBlock, 911, 404, 40);

                            BarCode = System.Text.ASCIIEncoding.ASCII.GetString(m_BarCode);
                            SNCode = System.Text.ASCIIEncoding.ASCII.GetString(m_SNCode);
                            ExpData = System.Text.ASCIIEncoding.ASCII.GetString(m_ExpData);

                            // update Report_DrumN set 条码 = '',批号 = '',有效期 = '' where 更换时间 = '' and 化学品名称 = '' and Drum名称 = ''

                            SQL_Ex = "update Report_DrumN set 条码 = '" + BarCode + "',批号 = '" + SNCode + "',有效期 = '" + ExpData + "' where 更换时间 = '" + stimeB.ToString() + "' and 化学品名称 = 'SYS9055A' and Drum名称 = 'DrumB'";

                            //SQL_Ex = "INSERT INTO Report_DrumN(更换时间,化学品名称,Drum名称,条码) Values('" + stime.ToString() + "','SYS9055A','" + ObjectN + "','" + BarCode + "')";


                            AsyncExecuteNonQuery(SQL_Ex, CallbackAsyncExecuteNonQuery);
                            SYS9055A.Write("DB911.DBX292.0", false);

                        }
                        DrumB_lastStep = 0;
                    }                    
                }

                await Task.Delay(1000);
            }

        }
        private static async Task GET_SYS9055B()
        {
            Plc SYS9055B = new Plc(CpuType.S71500, "10.249.76.48", 0, 1);
            string BarCode;
            string SNCode;
            string ExpData;
            string SQL_Ex;
            int DrumA_lastStep;
            int DrumB_lastStep;
            int Lorry_lastStep;
            DateTime stimeA, stimeB, stimeC, stimeD;
            DrumA_lastStep = 0;
            DrumB_lastStep = 0;
            Lorry_lastStep = 0;
            stimeA = DateTime.Now;
            stimeB = DateTime.Now;
            stimeC = DateTime.Now;
            stimeD = DateTime.Now;
            while (true)
            {

                if (SYS9055B.IsConnected == false)
                {
                    //Console.WriteLine("尝试连接SYS9055B PLC");
                    SYS9055B.OpenAsync().Wait(2000);
                }

                if (SYS9055B.IsConnected == true)
                {
                    var DrumA_Step = SYS9055B.Read("DB1105.DBW0");
                    var DrumB_Step = SYS9055B.Read("DB1205.DBW0");
                    if (DrumA_Step.ToString() == "3" && DrumA_lastStep == 0)
                    {
                        stimeA = DateTime.Now;
                        SQL_Ex = "exec Int_Drum '" + stimeA.ToString() + "','ACT940','DrumA'";
                        AsyncExecuteNonQuery(SQL_Ex, CallbackAsyncExecuteNonQuery);
                        DrumA_lastStep = 2;
                    }
                    if (DrumA_Step.ToString()  == "5" && DrumA_lastStep == 2)
                    {
                        var WriteBit = SYS9055B.Read("DB911.DBX292.0");
                        if (WriteBit.ToString() == "True")
                        {

                            var m_BarCode = SYS9055B.ReadBytes(DataType.DataBlock, 911, 324, 40);
                            var m_SNCode = SYS9055B.ReadBytes(DataType.DataBlock, 911, 364, 40);
                            var m_ExpData = SYS9055B.ReadBytes(DataType.DataBlock, 911, 404, 40);

                            BarCode = System.Text.ASCIIEncoding.ASCII.GetString(m_BarCode);
                            SNCode = System.Text.ASCIIEncoding.ASCII.GetString(m_SNCode);
                            ExpData = System.Text.ASCIIEncoding.ASCII.GetString(m_ExpData);

                            // update Report_DrumN set 条码 = '',批号 = '',有效期 = '' where 更换时间 = '' and 化学品名称 = '' and Drum名称 = ''

                            SQL_Ex = "update Report_DrumN set 条码 = '" + BarCode + "',批号 = '" + SNCode + "',有效期 = '" + ExpData + "' where 更换时间 = '" + stimeA.ToString() + "' and 化学品名称 = 'ACT940' and Drum名称 = 'DrumA'";

                            //SQL_Ex = "INSERT INTO Report_DrumN(更换时间,化学品名称,Drum名称,条码) Values('" + stime.ToString() + "','SYS9055B','" + ObjectN + "','" + BarCode + "')";


                            AsyncExecuteNonQuery(SQL_Ex, CallbackAsyncExecuteNonQuery);
                            SYS9055B.Write("DB911.DBX292.0", false);

                        }
                        DrumA_lastStep = 0;
                    }

                    if (DrumB_Step.ToString() == "3" && DrumB_lastStep == 0)
                    {
                        stimeB = DateTime.Now;
                        SQL_Ex = "exec Int_Drum '" + stimeB.ToString() + "','ACT940','DrumB'";
                        AsyncExecuteNonQuery(SQL_Ex, CallbackAsyncExecuteNonQuery);
                        DrumB_lastStep = 2;
                    }
                    if (DrumB_Step.ToString()  == "5" && DrumB_lastStep == 2)
                    {
                        var WriteBit = SYS9055B.Read("DB911.DBX292.0");
                        if (WriteBit.ToString() == "True")
                        {

                            var m_BarCode = SYS9055B.ReadBytes(DataType.DataBlock, 911, 324, 40);
                            var m_SNCode = SYS9055B.ReadBytes(DataType.DataBlock, 911, 364, 40);
                            var m_ExpData = SYS9055B.ReadBytes(DataType.DataBlock, 911, 404, 40);

                            BarCode = System.Text.ASCIIEncoding.ASCII.GetString(m_BarCode);
                            SNCode = System.Text.ASCIIEncoding.ASCII.GetString(m_SNCode);
                            ExpData = System.Text.ASCIIEncoding.ASCII.GetString(m_ExpData);

                            // update Report_DrumN set 条码 = '',批号 = '',有效期 = '' where 更换时间 = '' and 化学品名称 = '' and Drum名称 = ''

                            SQL_Ex = "update Report_DrumN set 条码 = '" + BarCode + "',批号 = '" + SNCode + "',有效期 = '" + ExpData + "' where 更换时间 = '" + stimeB.ToString() + "' and 化学品名称 = 'ACT940' and Drum名称 = 'DrumB'";

                            //SQL_Ex = "INSERT INTO Report_DrumN(更换时间,化学品名称,Drum名称,条码) Values('" + stime.ToString() + "','SYS9055B','" + ObjectN + "','" + BarCode + "')";


                            AsyncExecuteNonQuery(SQL_Ex, CallbackAsyncExecuteNonQuery);
                            SYS9055B.Write("DB911.DBX292.0", false);

                        }
                        DrumB_lastStep = 0;
                    }                    
                }

                await Task.Delay(1000);
            }

        }
        private static async Task GET_SYS9058A()
        {
            Plc SYS9058A = new Plc(CpuType.S71500, "10.249.76.50", 0, 1);
            string BarCode;
            string SNCode;
            string ExpData;
            string SQL_Ex;
            int DrumA_lastStep;
            int DrumB_lastStep;
            int Lorry_lastStep;
            DateTime stimeA, stimeB, stimeC, stimeD;
            DrumA_lastStep = 0;
            DrumB_lastStep = 0;
            Lorry_lastStep = 0;
            stimeA = DateTime.Now;
            stimeB = DateTime.Now;
            stimeC = DateTime.Now;
            stimeD = DateTime.Now;
            while (true)
            {

                if (SYS9058A.IsConnected == false)
                {
                    //Console.WriteLine("尝试连接SYS9058A PLC");
                    SYS9058A.OpenAsync().Wait(2000);
                }

                if (SYS9058A.IsConnected == true)
                {
                    var DrumA_Step = SYS9058A.Read("DB1105.DBW0");
                    var DrumB_Step = SYS9058A.Read("DB1205.DBW0");
                    if (DrumA_Step.ToString() == "3" && DrumA_lastStep == 0)
                    {
                        stimeA = DateTime.Now;
                        SQL_Ex = "exec Int_Drum '" + stimeA.ToString() + "','SYS9058A','DrumA'";
                        AsyncExecuteNonQuery(SQL_Ex, CallbackAsyncExecuteNonQuery);
                        DrumA_lastStep = 2;
                    }
                    if (DrumA_Step.ToString()  == "5" && DrumA_lastStep == 2)
                    {
                        var WriteBit = SYS9058A.Read("DB911.DBX292.0");
                        if (WriteBit.ToString() == "True")
                        {

                            var m_BarCode = SYS9058A.ReadBytes(DataType.DataBlock, 911, 324, 40);
                            var m_SNCode = SYS9058A.ReadBytes(DataType.DataBlock, 911, 364, 40);
                            var m_ExpData = SYS9058A.ReadBytes(DataType.DataBlock, 911, 404, 40);

                            BarCode = System.Text.ASCIIEncoding.ASCII.GetString(m_BarCode);
                            SNCode = System.Text.ASCIIEncoding.ASCII.GetString(m_SNCode);
                            ExpData = System.Text.ASCIIEncoding.ASCII.GetString(m_ExpData);

                            // update Report_DrumN set 条码 = '',批号 = '',有效期 = '' where 更换时间 = '' and 化学品名称 = '' and Drum名称 = ''

                            SQL_Ex = "update Report_DrumN set 条码 = '" + BarCode + "',批号 = '" + SNCode + "',有效期 = '" + ExpData + "' where 更换时间 = '" + stimeA.ToString() + "' and 化学品名称 = 'SYS9058A' and Drum名称 = 'DrumA'";

                            //SQL_Ex = "INSERT INTO Report_DrumN(更换时间,化学品名称,Drum名称,条码) Values('" + stime.ToString() + "','SYS9058A','" + ObjectN + "','" + BarCode + "')";


                            AsyncExecuteNonQuery(SQL_Ex, CallbackAsyncExecuteNonQuery);
                            SYS9058A.Write("DB911.DBX292.0", false);

                        }
                        DrumA_lastStep = 0;
                    }

                    if (DrumB_Step.ToString() == "3" && DrumB_lastStep == 0)
                    {
                        stimeB = DateTime.Now;
                        SQL_Ex = "exec Int_Drum '" + stimeB.ToString() + "','SYS9058A','DrumB'";
                        AsyncExecuteNonQuery(SQL_Ex, CallbackAsyncExecuteNonQuery);
                        DrumB_lastStep = 2;
                    }
                    if (DrumB_Step.ToString()  == "5" && DrumB_lastStep == 2)
                    {
                        var WriteBit = SYS9058A.Read("DB911.DBX292.0");
                        if (WriteBit.ToString() == "True")
                        {

                            var m_BarCode = SYS9058A.ReadBytes(DataType.DataBlock, 911, 324, 40);
                            var m_SNCode = SYS9058A.ReadBytes(DataType.DataBlock, 911, 364, 40);
                            var m_ExpData = SYS9058A.ReadBytes(DataType.DataBlock, 911, 404, 40);

                            BarCode = System.Text.ASCIIEncoding.ASCII.GetString(m_BarCode);
                            SNCode = System.Text.ASCIIEncoding.ASCII.GetString(m_SNCode);
                            ExpData = System.Text.ASCIIEncoding.ASCII.GetString(m_ExpData);

                            // update Report_DrumN set 条码 = '',批号 = '',有效期 = '' where 更换时间 = '' and 化学品名称 = '' and Drum名称 = ''

                            SQL_Ex = "update Report_DrumN set 条码 = '" + BarCode + "',批号 = '" + SNCode + "',有效期 = '" + ExpData + "' where 更换时间 = '" + stimeB.ToString() + "' and 化学品名称 = 'SYS9058A' and Drum名称 = 'DrumB'";

                            //SQL_Ex = "INSERT INTO Report_DrumN(更换时间,化学品名称,Drum名称,条码) Values('" + stime.ToString() + "','SYS9058A','" + ObjectN + "','" + BarCode + "')";


                            AsyncExecuteNonQuery(SQL_Ex, CallbackAsyncExecuteNonQuery);
                            SYS9058A.Write("DB911.DBX292.0", false);

                        }
                        DrumB_lastStep = 0;
                    }

                    
                }

                await Task.Delay(1000);
            }

        }
        private static async Task GET_SYS9058B()
        {
            Plc SYS9058B = new Plc(CpuType.S71500, "10.249.76.52", 0, 1);
            string BarCode;
            string SNCode;
            string ExpData;
            string SQL_Ex;
            int DrumA_lastStep;
            int DrumB_lastStep;
            int Lorry_lastStep;
            DateTime stimeA, stimeB, stimeC, stimeD;
            DrumA_lastStep = 0;
            DrumB_lastStep = 0;
            Lorry_lastStep = 0;
            stimeA = DateTime.Now;
            stimeB = DateTime.Now;
            stimeC = DateTime.Now;
            stimeD = DateTime.Now;
            while (true)
            {

                if (SYS9058B.IsConnected == false)
                {
                    //Console.WriteLine("尝试连接SYS9058B PLC");
                    SYS9058B.OpenAsync().Wait(2000);
                }

                if (SYS9058B.IsConnected == true)
                {
                    var DrumA_Step = SYS9058B.Read("DB1105.DBW0");
                    var DrumB_Step = SYS9058B.Read("DB1205.DBW0");
                    if (DrumA_Step.ToString() == "3" && DrumA_lastStep == 0)
                    {
                        stimeA = DateTime.Now;
                        SQL_Ex = "exec Int_Drum '" + stimeA.ToString() + "','SYS9058B','DrumA'";
                        AsyncExecuteNonQuery(SQL_Ex, CallbackAsyncExecuteNonQuery);
                        DrumA_lastStep = 2;
                    }
                    if (DrumA_Step.ToString()  == "5" && DrumA_lastStep == 2)
                    {
                        var WriteBit = SYS9058B.Read("DB911.DBX292.0");
                        if (WriteBit.ToString() == "True")
                        {

                            var m_BarCode = SYS9058B.ReadBytes(DataType.DataBlock, 911, 324, 40);
                            var m_SNCode = SYS9058B.ReadBytes(DataType.DataBlock, 911, 364, 40);
                            var m_ExpData = SYS9058B.ReadBytes(DataType.DataBlock, 911, 404, 40);

                            BarCode = System.Text.ASCIIEncoding.ASCII.GetString(m_BarCode);
                            SNCode = System.Text.ASCIIEncoding.ASCII.GetString(m_SNCode);
                            ExpData = System.Text.ASCIIEncoding.ASCII.GetString(m_ExpData);

                            // update Report_DrumN set 条码 = '',批号 = '',有效期 = '' where 更换时间 = '' and 化学品名称 = '' and Drum名称 = ''

                            SQL_Ex = "update Report_DrumN set 条码 = '" + BarCode + "',批号 = '" + SNCode + "',有效期 = '" + ExpData + "' where 更换时间 = '" + stimeA.ToString() + "' and 化学品名称 = 'SYS9058B' and Drum名称 = 'DrumA'";

                            //SQL_Ex = "INSERT INTO Report_DrumN(更换时间,化学品名称,Drum名称,条码) Values('" + stime.ToString() + "','SYS9058B','" + ObjectN + "','" + BarCode + "')";


                            AsyncExecuteNonQuery(SQL_Ex, CallbackAsyncExecuteNonQuery);
                            SYS9058B.Write("DB911.DBX292.0", false);

                        }
                        DrumA_lastStep = 0;
                    }

                    if (DrumB_Step.ToString() == "3" && DrumB_lastStep == 0)
                    {
                        stimeB = DateTime.Now;
                        SQL_Ex = "exec Int_Drum '" + stimeB.ToString() + "','SYS9058B','DrumB'";
                        AsyncExecuteNonQuery(SQL_Ex, CallbackAsyncExecuteNonQuery);
                        DrumB_lastStep = 2;
                    }
                    if (DrumB_Step.ToString()  == "5" && DrumB_lastStep == 2)
                    {
                        var WriteBit = SYS9058B.Read("DB911.DBX292.0");
                        if (WriteBit.ToString() == "True")
                        {

                            var m_BarCode = SYS9058B.ReadBytes(DataType.DataBlock, 911, 324, 40);
                            var m_SNCode = SYS9058B.ReadBytes(DataType.DataBlock, 911, 364, 40);
                            var m_ExpData = SYS9058B.ReadBytes(DataType.DataBlock, 911, 404, 40);

                            BarCode = System.Text.ASCIIEncoding.ASCII.GetString(m_BarCode);
                            SNCode = System.Text.ASCIIEncoding.ASCII.GetString(m_SNCode);
                            ExpData = System.Text.ASCIIEncoding.ASCII.GetString(m_ExpData);

                            // update Report_DrumN set 条码 = '',批号 = '',有效期 = '' where 更换时间 = '' and 化学品名称 = '' and Drum名称 = ''

                            SQL_Ex = "update Report_DrumN set 条码 = '" + BarCode + "',批号 = '" + SNCode + "',有效期 = '" + ExpData + "' where 更换时间 = '" + stimeB.ToString() + "' and 化学品名称 = 'SYS9058B' and Drum名称 = 'DrumB'";

                            //SQL_Ex = "INSERT INTO Report_DrumN(更换时间,化学品名称,Drum名称,条码) Values('" + stime.ToString() + "','SYS9058B','" + ObjectN + "','" + BarCode + "')";


                            AsyncExecuteNonQuery(SQL_Ex, CallbackAsyncExecuteNonQuery);
                            SYS9058B.Write("DB911.DBX292.0", false);

                        }
                        DrumB_lastStep = 0;
                    }                    
                }

                await Task.Delay(1000);
            }

        }
        private static async Task GET_NMPA()
        {
            Plc NMPA = new Plc(CpuType.S71500, "10.249.76.54", 0, 1);
            string BarCode;
            string SNCode;
            string ExpData;
            string SQL_Ex;
            int DrumA_lastStep;
            int DrumB_lastStep;
            int Lorry_lastStep;
            DateTime stimeA, stimeB, stimeC, stimeD;
            DrumA_lastStep = 0;
            DrumB_lastStep = 0;
            Lorry_lastStep = 0;
            stimeA = DateTime.Now;
            stimeB = DateTime.Now;
            stimeC = DateTime.Now;
            stimeD = DateTime.Now;
            while (true)
            {

                if (NMPA.IsConnected == false)
                {
                    //Console.WriteLine("尝试连接NMPA PLC");
                    NMPA.OpenAsync().Wait(2000);
                }

                if (NMPA.IsConnected == true)
                {
                    var DrumA_Step = NMPA.Read("DB1105.DBW0");
                    var DrumB_Step = NMPA.Read("DB1205.DBW0");
                    if (DrumA_Step.ToString() == "3" && DrumA_lastStep == 0)
                    {
                        stimeA = DateTime.Now;
                        SQL_Ex = "exec Int_Drum '" + stimeA.ToString() + "','NMPA','DrumA'";
                        AsyncExecuteNonQuery(SQL_Ex, CallbackAsyncExecuteNonQuery);
                        DrumA_lastStep = 2;
                    }
                    if (DrumA_Step.ToString()  == "5" && DrumA_lastStep == 2)
                    {
                        var WriteBit = NMPA.Read("DB911.DBX292.0");
                        if (WriteBit.ToString() == "True")
                        {

                            var m_BarCode = NMPA.ReadBytes(DataType.DataBlock, 911, 324, 40);
                            var m_SNCode = NMPA.ReadBytes(DataType.DataBlock, 911, 364, 40);
                            var m_ExpData = NMPA.ReadBytes(DataType.DataBlock, 911, 404, 40);

                            BarCode = System.Text.ASCIIEncoding.ASCII.GetString(m_BarCode);
                            SNCode = System.Text.ASCIIEncoding.ASCII.GetString(m_SNCode);
                            ExpData = System.Text.ASCIIEncoding.ASCII.GetString(m_ExpData);

                            // update Report_DrumN set 条码 = '',批号 = '',有效期 = '' where 更换时间 = '' and 化学品名称 = '' and Drum名称 = ''

                            SQL_Ex = "update Report_DrumN set 条码 = '" + BarCode + "',批号 = '" + SNCode + "',有效期 = '" + ExpData + "' where 更换时间 = '" + stimeA.ToString() + "' and 化学品名称 = 'NMPA' and Drum名称 = 'DrumA'";

                            //SQL_Ex = "INSERT INTO Report_DrumN(更换时间,化学品名称,Drum名称,条码) Values('" + stime.ToString() + "','NMPA','" + ObjectN + "','" + BarCode + "')";


                            AsyncExecuteNonQuery(SQL_Ex, CallbackAsyncExecuteNonQuery);
                            NMPA.Write("DB911.DBX292.0", false);

                        }
                        DrumA_lastStep = 0;
                    }

                    if (DrumB_Step.ToString() == "3" && DrumB_lastStep == 0)
                    {
                        stimeB = DateTime.Now;
                        SQL_Ex = "exec Int_Drum '" + stimeB.ToString() + "','NMPA','DrumB'";
                        AsyncExecuteNonQuery(SQL_Ex, CallbackAsyncExecuteNonQuery);
                        DrumB_lastStep = 2;
                    }
                    if (DrumB_Step.ToString()  == "5" && DrumB_lastStep == 2)
                    {
                        var WriteBit = NMPA.Read("DB911.DBX292.0");
                        if (WriteBit.ToString() == "True")
                        {

                            var m_BarCode = NMPA.ReadBytes(DataType.DataBlock, 911, 324, 40);
                            var m_SNCode = NMPA.ReadBytes(DataType.DataBlock, 911, 364, 40);
                            var m_ExpData = NMPA.ReadBytes(DataType.DataBlock, 911, 404, 40);

                            BarCode = System.Text.ASCIIEncoding.ASCII.GetString(m_BarCode);
                            SNCode = System.Text.ASCIIEncoding.ASCII.GetString(m_SNCode);
                            ExpData = System.Text.ASCIIEncoding.ASCII.GetString(m_ExpData);

                            // update Report_DrumN set 条码 = '',批号 = '',有效期 = '' where 更换时间 = '' and 化学品名称 = '' and Drum名称 = ''

                            SQL_Ex = "update Report_DrumN set 条码 = '" + BarCode + "',批号 = '" + SNCode + "',有效期 = '" + ExpData + "' where 更换时间 = '" + stimeB.ToString() + "' and 化学品名称 = 'NMPA' and Drum名称 = 'DrumB'";

                            //SQL_Ex = "INSERT INTO Report_DrumN(更换时间,化学品名称,Drum名称,条码) Values('" + stime.ToString() + "','NMPA','" + ObjectN + "','" + BarCode + "')";


                            AsyncExecuteNonQuery(SQL_Ex, CallbackAsyncExecuteNonQuery);
                            NMPA.Write("DB911.DBX292.0", false);

                        }
                        DrumB_lastStep = 0;
                    }                    

                }

                await Task.Delay(1000);
            }

        }
        private static async Task GET_NMPB()
        {
            Plc NMPB = new Plc(CpuType.S71500, "10.249.76.56", 0, 1);
            string BarCode;
            string SNCode;
            string ExpData;
            string SQL_Ex;
            int DrumA_lastStep;
            int DrumB_lastStep;
            int Lorry_lastStep;
            DateTime stimeA, stimeB, stimeC, stimeD;
            DrumA_lastStep = 0;
            DrumB_lastStep = 0;
            Lorry_lastStep = 0;
            stimeA = DateTime.Now;
            stimeB = DateTime.Now;
            stimeC = DateTime.Now;
            stimeD = DateTime.Now;
            while (true)
            {

                if (NMPB.IsConnected == false)
                {
                    //Console.WriteLine("尝试连接NMPB PLC");
                    NMPB.OpenAsync().Wait(2000);
                }

                if (NMPB.IsConnected == true)
                {
                    var DrumA_Step = NMPB.Read("DB1105.DBW0");
                    var DrumB_Step = NMPB.Read("DB1205.DBW0");
                    if (DrumA_Step.ToString() == "3" && DrumA_lastStep == 0)
                    {
                        stimeA = DateTime.Now;
                        SQL_Ex = "exec Int_Drum '" + stimeA.ToString() + "','NMPB','DrumA'";
                        AsyncExecuteNonQuery(SQL_Ex, CallbackAsyncExecuteNonQuery);
                        DrumA_lastStep = 2;
                    }
                    if (DrumA_Step.ToString()  == "5" && DrumA_lastStep == 2)
                    {
                        var WriteBit = NMPB.Read("DB911.DBX292.0");
                        if (WriteBit.ToString() == "True")
                        {

                            var m_BarCode = NMPB.ReadBytes(DataType.DataBlock, 911, 324, 40);
                            var m_SNCode = NMPB.ReadBytes(DataType.DataBlock, 911, 364, 40);
                            var m_ExpData = NMPB.ReadBytes(DataType.DataBlock, 911, 404, 40);

                            BarCode = System.Text.ASCIIEncoding.ASCII.GetString(m_BarCode);
                            SNCode = System.Text.ASCIIEncoding.ASCII.GetString(m_SNCode);
                            ExpData = System.Text.ASCIIEncoding.ASCII.GetString(m_ExpData);

                            // update Report_DrumN set 条码 = '',批号 = '',有效期 = '' where 更换时间 = '' and 化学品名称 = '' and Drum名称 = ''

                            SQL_Ex = "update Report_DrumN set 条码 = '" + BarCode + "',批号 = '" + SNCode + "',有效期 = '" + ExpData + "' where 更换时间 = '" + stimeA.ToString() + "' and 化学品名称 = 'NMPB' and Drum名称 = 'DrumA'";

                            //SQL_Ex = "INSERT INTO Report_DrumN(更换时间,化学品名称,Drum名称,条码) Values('" + stime.ToString() + "','NMPB','" + ObjectN + "','" + BarCode + "')";


                            AsyncExecuteNonQuery(SQL_Ex, CallbackAsyncExecuteNonQuery);
                            NMPB.Write("DB911.DBX292.0", false);

                        }
                        DrumA_lastStep = 0;
                    }

                    if (DrumB_Step.ToString() == "3" && DrumB_lastStep == 0)
                    {
                        stimeB = DateTime.Now;
                        SQL_Ex = "exec Int_Drum '" + stimeB.ToString() + "','NMPB','DrumB'";
                        AsyncExecuteNonQuery(SQL_Ex, CallbackAsyncExecuteNonQuery);
                        DrumB_lastStep = 2;
                    }
                    if (DrumB_Step.ToString()  == "5" && DrumB_lastStep == 2)
                    {
                        var WriteBit = NMPB.Read("DB911.DBX292.0");
                        if (WriteBit.ToString() == "True")
                        {

                            var m_BarCode = NMPB.ReadBytes(DataType.DataBlock, 911, 324, 40);
                            var m_SNCode = NMPB.ReadBytes(DataType.DataBlock, 911, 364, 40);
                            var m_ExpData = NMPB.ReadBytes(DataType.DataBlock, 911, 404, 40);

                            BarCode = System.Text.ASCIIEncoding.ASCII.GetString(m_BarCode);
                            SNCode = System.Text.ASCIIEncoding.ASCII.GetString(m_SNCode);
                            ExpData = System.Text.ASCIIEncoding.ASCII.GetString(m_ExpData);

                            // update Report_DrumN set 条码 = '',批号 = '',有效期 = '' where 更换时间 = '' and 化学品名称 = '' and Drum名称 = ''

                            SQL_Ex = "update Report_DrumN set 条码 = '" + BarCode + "',批号 = '" + SNCode + "',有效期 = '" + ExpData + "' where 更换时间 = '" + stimeB.ToString() + "' and 化学品名称 = 'NMPB' and Drum名称 = 'DrumB'";

                            //SQL_Ex = "INSERT INTO Report_DrumN(更换时间,化学品名称,Drum名称,条码) Values('" + stime.ToString() + "','NMPB','" + ObjectN + "','" + BarCode + "')";


                            AsyncExecuteNonQuery(SQL_Ex, CallbackAsyncExecuteNonQuery);
                            NMPB.Write("DB911.DBX292.0", false);

                        }
                        DrumB_lastStep = 0;
                    }                  


                }

                await Task.Delay(1000);
            }

        }

        /// <summary>
        /// 异步执行SQL。 
        /// </summary>
        /// <param name="sqlText">要执行的SQLText</param>
        /// <param name="callBack">回执行监控事件</param>
        public static void AsyncExecuteNonQuery(string sqlText, AsyncCallback callBack)
        {
            //关闭数据库连接要在callback中关闭，因为是异步操作
            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection();
                connection.ConnectionString = "Data Source = 127.0.0.1; Initial Catalog = Gentech; User ID = sa; Pwd = sa123456";
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sqlText;
                connection.Open();
                cmd.BeginExecuteNonQuery(callBack, cmd); //开始执行SQL语句
            }
            catch (Exception ex)
            {
                if (connection != null)
                {
                    connection.Close();
                }
            }
        }

        public static void CallbackAsyncExecuteNonQuery(IAsyncResult callBack)
        {
            SqlCommand cmm = null;
            try
            {
                cmm = (SqlCommand)callBack.AsyncState;
                if (cmm == null)
                {
                    return;
                }
                cmm.EndExecuteNonQuery(callBack); //执行完毕
            }
            catch (Exception ex)
            {
                if (cmm != null)
                {
                    //异步执行SQL异常
                    cmm.Dispose();
                }
                else
                {
                    //异步执行SQL异常
                }
            }
            finally
            {
                if (cmm != null && cmm.Connection != null && cmm.Connection.State != ConnectionState.Closed)
                {
                    cmm.Dispose();
                    cmm.Connection.Close();
                }
            }
        }


    }
}
