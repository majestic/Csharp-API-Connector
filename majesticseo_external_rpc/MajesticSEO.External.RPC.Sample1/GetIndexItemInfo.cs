﻿
/// <copyright>
/// 
/// Version 0.9.3
/// 
/// Copyright (c) 2011, Majestic-12 Ltd
/// All rights reserved.
/// 
/// Redistribution and use in source and binary forms, with or without
/// modification, are permitted provided that the following conditions are met:
///     1. Redistributions of source code must retain the above copyright
///        notice, this list of conditions and the following disclaimer.
///     2. Redistributions in binary form must reproduce the above copyright
///        notice, this list of conditions and the following disclaimer in the
///        documentation and/or other materials provided with the distribution.
///     3. Neither the name of the Majestic-12 Ltd nor the
///        names of its contributors may be used to endorse or promote products
///        derived from this software without specific prior written permission.
///        
/// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
/// ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
/// WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
/// DISCLAIMED. IN NO EVENT SHALL Majestic-12 Ltd BE LIABLE FOR ANY
/// DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
/// (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
/// LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
/// ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
/// (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
/// SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
/// 
/// </copyright>

/* NOTE: The code below is specifically for the GetIndexItemInfo API command
 *       For other API commands, the arguments required may differ.
 *       Please refer to the Majestic SEO Developer Wiki for more information
 *       regarding other API commands and their arguments.
 */

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace MajesticSEO.External.RPC
{
    public class GetIndexItemInfo
    {
        public static void Main(string[] args)
        {
            string endpoint = "http://enterprise.majesticseo.com/api_command";

            Console.WriteLine("\n***********************************************************"
                + "*****************");

            Console.WriteLine("\nEndpoint: " + endpoint);

            if("http://enterprise.majesticseo.com/api_command".Equals(endpoint)) 
            {
                Console.WriteLine("\nThis program is hard-wired to the Enterprise API.");

                Console.WriteLine("\nIf you do not have access to the Enterprise API, "
                    + "change the endpoint to: \nhttp://developer.majesticseo.com/api_command.");
            } 
            else 
            {
                Console.WriteLine("\nThis program is hard-wired to the Developer API "
                    + "and hence the subset of data \nreturned will be substantially "
                    + "smaller than that which will be returned from \neither the "
                    + "Enterprise API or the Majestic SEO website.");

                Console.WriteLine("\nTo make this program use the Enterprise API, change "
                    + "the endpoint to: \nhttp://enterprise.majesticseo.com/api_command.");
            }

            Console.WriteLine("\n***********************************************************"
                    + "*****************");

            Console.WriteLine(
                "\n\nThis example program will return key information about \"index items\"."
                + "\n\nThe following must be provided in order to run this program: "
                + "\n1. API key, \n2. List of items to query"
                + "\n\nPlease enter your API key:");

            string app_api_key = Console.ReadLine();

            Console.WriteLine(
                "\nPlease enter the list of items you wish to query seperated by "
                + "commas: \n(e.g. majesticseo.com, majestic12.co.uk)");

            string itemsToQuery = Console.ReadLine();
            string[] items = Regex.Split(itemsToQuery, ", ");

            /* create a Dictionary from the resulting array with the key being
             * "item0 => first item to query, item1 => second item to query" etc */
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            for (int i = 0; i < items.Length; i++)
            {
                parameters.Add("item" + i, items[i]);
            }

            // add the total number of items to the Dictionary with the key being "items"
            parameters.Add("items", items.Length.ToString());

            parameters.Add("datasource", "fresh");

            APIService apiService = new APIService(app_api_key, endpoint);
            Response response = apiService.ExecuteCommand("GetIndexItemInfo", parameters);

            // check the response code
            if(response.IsOK())
            {
                // print the results table
                DataTable results = response.GetTableForName("Results");

                foreach(Dictionary<string, string> row in results.GetTableRows())
                {
                    string item = row["Item"];
                    Console.WriteLine("\n<" + item + ">");

                    List<string> keys = new List<string>(row.Keys);
                    keys.Sort();

                    foreach(string key in keys)
                    {
                        if(!key.Equals("item"))
                        {
                            string value = row[key];
                            Console.WriteLine(" " + key + " ... " + value);
                        }
                    }
                }

                if("http://developer.majesticseo.com/api_command".Equals(endpoint)) 
                {
                    Console.WriteLine("\n\n***********************************************************"
                        + "*****************");

                    Console.WriteLine("\nEndpoint: " + endpoint);

                    Console.WriteLine("\nThis program is hard-wired to the Developer API "
                        + "and hence the subset of data \nreturned will be substantially "
                        + "smaller than that which will be returned from \neither the "
                        + "Enterprise API or the Majestic SEO website.");

                    Console.WriteLine("\nTo make this program use the Enterprise API, change "
                        + "the endpoint to: \nhttp://enterprise.majesticseo.com/api_command.");

                    Console.WriteLine("\n***********************************************************"
                        + "*****************");
                }
            }
            else
            {
                Console.WriteLine("\nERROR MESSAGE:");
                Console.WriteLine(response.GetErrorMessage());

                Console.WriteLine("\n\n***********************************************************"
                    + "*****************");

                Console.WriteLine("\nDebugging Info:");
                Console.WriteLine("\n  Endpoint: \t" + endpoint);
                Console.WriteLine("  API Key: \t" + app_api_key);

                if("http://enterprise.majesticseo.com/api_command".Equals(endpoint)) 
                {
                    Console.WriteLine("\n  Is this API Key valid for this Endpoint?");

                    Console.WriteLine("\n  This program is hard-wired to the Enterprise API.");

                    Console.WriteLine("\n  If you do not have access to the Enterprise API, "
                       + "change the endpoint to: \n  http://developer.majesticseo.com/api_command.");
                }

                Console.WriteLine("\n***********************************************************"
                    + "*****************");
            }

            Console.Read();
        }
    }
}
