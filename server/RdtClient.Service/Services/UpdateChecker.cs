﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace RdtClient.Service.Services
{
    public class UpdateChecker : BackgroundService
    {
        public static String CurrentVersion { get; private set; }
        public static String LatestVersion { get; private set; }

        private readonly ILogger<TaskRunner> _logger;

        public UpdateChecker(ILogger<TaskRunner> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var version = Assembly.GetEntryAssembly()?.GetName().Version?.ToString();

            if (String.IsNullOrWhiteSpace(version))
            {
                CurrentVersion = "";

                return;
            }

            CurrentVersion = $"v{version[..version.LastIndexOf(".", StringComparison.Ordinal)]}";

            _logger.LogInformation("UpdateChecker started, currently on version {CurrentVersion}.", CurrentVersion);

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var httpClient = new HttpClient();
                    httpClient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("RdtClient", CurrentVersion));
                    var response = await httpClient.GetStringAsync($"https://api.github.com/repos/rogerfar/rdt-client/tags?per_page=1", stoppingToken);

                    var gitHubReleases = JsonConvert.DeserializeObject<List<GitHubReleasesResponse>>(response);

                    if (gitHubReleases == null || gitHubReleases.Count == 0)
                    {
                        return;
                    }

                    var latestRelease = gitHubReleases.First().Name;

                    if (latestRelease != CurrentVersion)
                    {
                        _logger.LogInformation("New version found on GitHub: {latestRelease}", latestRelease);
                    }

                    LatestVersion = latestRelease;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Unexpected error occurred in TorrentDownloadManager.Tick");
                }

                await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
            }

            _logger.LogInformation("UpdateChecker stopped.");
        }
    }

    public class GitHubReleasesResponse 
    {
        [JsonProperty("name")]
        public String Name { get; set; }
    }
}
