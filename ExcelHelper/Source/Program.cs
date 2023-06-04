using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Windows.Forms;
using Perforce.P4;

using DataHandler;
using ExcelManager;

namespace ExcelHelper
{

    internal static class Program
    {
        /// <summary>
        /// 해당 애플리케이션의 주 진입점입니다.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ExcelHelperMain());
        }
    }
}



//class ConPerforce
//{
//    static void Main(string[] args)
//    {
//        string uri = "localhost:1666";
//        string user = "admin";
//        string ws_client = "admin_space";
        
//        Server server = new Server(new ServerAddress(uri));
//        Repository rep = new Repository(server);
//        Connection con = rep.Connection;

//        con.UserName = user;
//        con.Client = new Client();
//        con.Client.Name = ws_client;

//        con.Connect(null);
//        //// Perforce 서버 연결 정보
//        //Connection conn = new Connection();
//        //conn.Port = "perforce:1666";
//        //conn.User = "username";
//        //conn.Client = "clientname";
//        //conn.Password = "password";

//        //// 연결
//        //conn.Connect();
//        //if (conn.connected)
//        //{
//        //    Console.WriteLine("Connected to Perforce");

//        //    // 특정 리비전으로 클라이언트 모드 변경
//        //    Workspace ws = new Workspace(conn);
//        //    ws.Client = new Client();
//        //    ws.Client.Name = conn.Client;
//        //    ws.Ready = true;
//        //    ws.SetClient(ws.Client);

//        //    // 리비전 번호
//        //    int revNum = 12345;

//        //    // 파일 목록 가져오기
//        //    Options options = new Options();
//        //    options["-a"] = null; // 변경된 모든 파일 가져오기
//        //    FileSpec filespec = new FileSpec(new DepotPath("//depot/..."), null, new Revision(revNum));
//        //    IList<FileMetaData> files = ws.GetFileMetaData(filespec, options);

//        //    // 파일 열기
//        //    foreach (FileMetaData file in files)
//        //    {
//        //        if (!file.HeadAction.Equals("delete"))
//        //        {
//        //            FileSpec fileSpec = new FileSpec(new DepotPath(file.DepotPath), null, new Revision(revNum));
//        //            Options syncOptions = new Options(SyncFilesCmdFlags.Force);
//        //            FileSpec[] syncedFiles = ws.SyncFiles(syncOptions, fileSpec);
//        //        }
//        //    }

//        //    Console.WriteLine("Files synced successfully");

//        //    // 연결 해제
//        //    conn.Disconnect();
//        //}
//        //else
//        //{
//        //    Console.WriteLine("Failed to connect to Perforce");
//        //}
//    }
//}