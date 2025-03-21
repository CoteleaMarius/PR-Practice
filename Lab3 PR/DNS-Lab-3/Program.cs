using System.Net;
using DnsClient;

namespace DNS_Lab_3;

internal abstract class DnsResolverApp
{
    private static LookupClient? _customClient;
    private static IPAddress? _customDnsIp;

    static async Task Main()
    {
        Console.WriteLine("DNS Resolver App. Commands: 'resolve <domain/ip>', 'use dns <ip>', 'use default', 'exit'");

        while (true)
        {
            Console.Write("> ");
            string? userInput = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(userInput)) continue;

            var commandParts = userInput.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (commandParts[0].ToLower() == "exit") break;

            try
            {
                if (commandParts[0].ToLower() == "resolve" && commandParts.Length == 2)
                {
                    await HandleResolve(commandParts[1]);
                }
                else if (commandParts[0].ToLower() == "use")
                {
                    if (commandParts.Length == 3 && commandParts[1].ToLower() == "dns")
                    {
                        await ConfigureCustomDns(commandParts[2]);
                    }
                    else if (commandParts.Length == 2 && commandParts[1].ToLower() == "default")
                    {
                        ResetToSystemDns();
                    }
                    else
                    {
                        Console.WriteLine("Invalid 'use' command. Use 'use dns <ip>' or 'use default'.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid command. Use 'resolve <domain/ip>', 'use dns <ip>', 'use default'.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }

    static async Task HandleResolve(string query)
    {
        if (IPAddress.TryParse(query, out IPAddress? ipAddress))
        {
            await PerformReverseLookup(ipAddress);
        }
        else
        {
            await PerformForwardLookup(query);
        }
    }

    static async Task PerformForwardLookup(string domainName)
    {
        var timer = System.Diagnostics.Stopwatch.StartNew();
        if (_customClient == null)
        {
            try
            {
                IPAddress[] addresses = await Dns.GetHostAddressesAsync(domainName);
                timer.Stop();
                if (addresses.Length == 0)
                {
                    Console.WriteLine("Could not resolve domain.");
                    return;
                }
                Console.WriteLine($"Resolved IP addresses for {domainName} (system DNS, {timer.ElapsedMilliseconds} ms):");
                foreach (var address in addresses)
                {
                    Console.WriteLine(address);
                }
            }
            catch
            {
                timer.Stop();
                Console.WriteLine("Could not resolve domain.");
            }
        }
        else
        {
            try
            {
                var result = await _customClient.QueryAsync(domainName, QueryType.A);
                timer.Stop();
                if (result.HasError || !result.Answers.Any())
                {
                    Console.WriteLine("Could not resolve domain.");
                    return;
                }
                Console.WriteLine($"Resolved IP addresses for {domainName} (custom DNS {_customDnsIp}, {timer.ElapsedMilliseconds} ms):");
                foreach (var record in result.Answers.ARecords())
                {
                    Console.WriteLine(record.Address);
                }
            }
            catch
            {
                timer.Stop();
                Console.WriteLine("Could not resolve domain with custom DNS.");
            }
        }
    }

    static async Task PerformReverseLookup(IPAddress? ipAddress)
    {
        var timer = System.Diagnostics.Stopwatch.StartNew();
        if (_customClient == null)
        {
            try
            {
                if (ipAddress != null)
                {
                    IPHostEntry entry = await Dns.GetHostEntryAsync(ipAddress);
                    timer.Stop();
                    if (string.IsNullOrEmpty(entry.HostName) && entry.Aliases.Length == 0)
                    {
                        Console.WriteLine("Could not resolve IP.");
                        return;
                    }
                    Console.WriteLine($"Resolved domain(s) for {ipAddress} (system DNS, {timer.ElapsedMilliseconds} ms):");
                    Console.WriteLine(entry.HostName);
                    foreach (var alias in entry.Aliases)
                    {
                        Console.WriteLine(alias);
                    }
                }
            }
            catch
            {
                timer.Stop();
                Console.WriteLine("Could not resolve IP.");
            }
        }
        else
        {
            try
            {
                var result = await _customClient.QueryReverseAsync(ipAddress);
                timer.Stop();
                if (result.HasError || !result.Answers.Any())
                {
                    Console.WriteLine("Could not resolve IP.");
                    return;
                }
                Console.WriteLine($"Resolved domain(s) for {ipAddress} (custom DNS {_customDnsIp}, {timer.ElapsedMilliseconds} ms):");
                foreach (var record in result.Answers.PtrRecords())
                {
                    Console.WriteLine(record.PtrDomainName);
                }
            }
            catch
            {
                timer.Stop();
                Console.WriteLine("Could not resolve IP with custom DNS.");
            }
        }
    }

    static async Task ConfigureCustomDns(string dnsIpAddress)
    {
        if (!IPAddress.TryParse(dnsIpAddress, out IPAddress? parsedIp))
        {
            Console.WriteLine("Invalid DNS server IP address.");
            return;
        }

        try
        {
            _customClient = new LookupClient(new LookupClientOptions(parsedIp)
            {
                Timeout = TimeSpan.FromSeconds(3),
                UseCache = false
            });
            _customDnsIp = parsedIp;
            Console.WriteLine($"Custom DNS set: {_customDnsIp}");

            var test = await _customClient.QueryAsync("example.com", QueryType.A);
            Console.WriteLine(test.HasError
                ? "Warning: The custom DNS server may not be responding correctly."
                : "Custom DNS server responded successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to set custom DNS: {ex.Message}");
            _customClient = null;
            _customDnsIp = null;
        }
    }

    static void ResetToSystemDns()
    {
        _customClient = null;
        _customDnsIp = null;
        Console.WriteLine("Reverted to system DNS.");
    }
}