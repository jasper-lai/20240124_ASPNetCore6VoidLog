﻿namespace ASPNetCore6VoidLog.Services
{
    using ASPNetCore6VoidLog.Controllers;
    using ASPNetCore6VoidLog.ViewModels;
    using ASPNetCore6VoidLog.Wrapper;
    using Microsoft.Extensions.Options;
    using System.IO.Abstractions;
    using System.Runtime.InteropServices;
    using System.Text.Encodings.Web;
    using System.Text.Json;
    using System.Text.Unicode;

    public class LottoService : ILottoService
    {
        private readonly IRandomGenerator _randomGenerator;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IFileSystem _fileSystem;
        private readonly ILogger<LottoService> _logger;

        public LottoService(IRandomGenerator randomGenerator, IDateTimeProvider dateTimeProvider
                          , IFileSystem fileSystem, ILogger<LottoService> logger) 
        {
            _randomGenerator = randomGenerator;
            _dateTimeProvider = dateTimeProvider;
            _fileSystem = fileSystem;
            _logger = logger;
        }

        public LottoViewModel Lottoing(int min, int max)
        {

            var result = new LottoViewModel();
            var jsonOptions = new JsonSerializerOptions()
            {

                ////中文字不編碼
                //Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                //允許基本拉丁英文及中日韓文字維持原字元
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.CjkUnifiedIdeographs),

                //不進行換行與縮排
                WriteIndented = false,

                //字首處理小寫
                //PropertyNamingPolicy = null    //不轉小寫
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase   // 轉小寫
            };

            string myJson;


            // -----------------------
            // 檢核1: 是否為每個月 5 日
            // -----------------------
            var now = _dateTimeProvider.GetCurrentTime();
            if (now.Day != 5)
            {
                //result.Sponsor = string.Empty;
                //result.YourNumber = -1;
                //result.Message = "非每個月5日, 不開獎";

                ////序列化 (by System.Text.Json) 後寫到 Log
                //myJson = JsonSerializer.Serialize(result, jsonOptions);
                ////#pragma warning disable CA2254 // Template should be a static expression
                ////                _logger.LogCritical(myJson);
                ////#pragma warning restore CA2254 // Template should be a static expression
                //_logger.LogCritical("{myJson}", myJson);
                //return result;
            }

            // -----------------------
            // 檢核2: 主辦人員是否已按下[開始]按鈕
            // -----------------------
            // 註: 這裡有可能會出現一些 Exception, 例如: FileNotFoundException
            var sponsor = string.Empty;
            try
            {
                sponsor = _fileSystem.File.ReadAllText("Extras/startup.txt");
            }
            catch (Exception)
            {
                result.Sponsor = sponsor;
                result.YourNumber = -2;
                result.Message = "主辦人員尚未按下[開始]按鈕";

                //序列化 (by System.Text.Json) 後寫到 Log
                myJson = JsonSerializer.Serialize(result, jsonOptions);
                _logger.LogError("{myJson}", myJson);
                //
                return result;
            }

            // Random(min, max): 含下界, 不含上界
            var yourNumber = _randomGenerator.Next(min, max);
            // 只要餘數是 9, 就代表中獎
            var message = (yourNumber % 10 == 9) ? "恭喜中獎" : "再接再厲";
            result.Sponsor = sponsor;
            result.YourNumber = yourNumber;
            result.Message = message;

            //序列化 (by System.Text.Json) 後寫到 Log
            myJson = JsonSerializer.Serialize(result, jsonOptions);
            _logger.LogInformation("{myJson}", myJson);
            //
            return result;
        }
    }
}
