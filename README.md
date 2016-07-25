# twMVC #23 Azure Functions 範例程式

## 活動

twMVC #23 於 2016-7-24 舉辦, 詳情可參考[活動資訊與報名](https://mvc.tw/event/2016/7/24)網頁。

## 原始碼使用說明

此處原始碼皆以 C# Script 開發。

Fork 這個專案，在新的 Function App 中，加入這個專案為 CI 來源。

## 範例目錄

- PeriodTimer  
  定時觸發器，作為 Azure Functions 第一個練習，學習重點：熟悉 Function App 建立與線上設定、開發介面，進行第一個事件驅動的 Function。

- MakeThumb  
  Blob 觸發器，學習重點：進一步暸解 Binding，熟悉 C# 的 Function App 開發細節，並利用 GitHub 作為 Function App 的部署方式。

- SlashTime  
  給 Slack 的 Slash Command 用的 HTTP 觸發器。

- SlashCompany  
  給 Slack 的 Slash Command 用的 HTTP 觸發器。

- SlashMoedict  
  給 Slack 的 Slash Command 用的 HTTP 觸發器。

```
Slack 目錄僅作存放類別定義用，不是一個完整的 Azure Function
```

## C# 開發者

開發者為 Function 安裝增加 NuGet 套件的作法請參考這篇[Stackoverflow 上的問答](http://stackoverflow.com/questions/36411536/how-can-i-use-nuget-packages-in-my-azure-functions)。

## 參考資料

- https://github.com/Azure/AzureFunctions
- https://github.com/Azure/azure-webjobs-sdk-templates/tree/master/Templates
- https://github.com/Azure/azure-content/tree/master/articles/azure-functions
- https://github.com/Azure/azure-content-zhtw/tree/master/articles/azure-functions
