using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using Dropbox.Api;
using System.Linq;

namespace ClassLibrary
{
    public class DropboxClass
    {
        public static string Token { get; set; } 

        public static async Task Run()
        {
            string RealFile = "";
            string folder = "/Apps/SMSBackupRestore/";
            using (StreamReader reader = new StreamReader(@"C:\Users\totha\Source\Repos\LibraryiAboutMoney\Dropbox\token.dat"))
            {
                Token = reader.ReadLine();
            }

            using (var dbx = new DropboxClient(Token))
            {
                var list = await dbx.Files.ListFolderAsync(folder);

                foreach (var item in list.Entries.Where(i => i.IsFile))
                {
                    RealFile = item.Name;
                }
            }


            using (var dbx = new DropboxClient(Token))
            {
                using (var response = await dbx.Files.DownloadAsync(folder + RealFile))
                {
                    var s = response.GetContentAsByteArrayAsync();
                    s.Wait();
                    var d = s.Result;

                    File.WriteAllBytes(RealFile, d);
                }
            }
        }
    }
}
