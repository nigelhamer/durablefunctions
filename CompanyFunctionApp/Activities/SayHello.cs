// Copyright (c) .NET Foundation. All rights reserved.
using Microsoft.Azure.WebJobs;

namespace CompanyFunctionApp.Activities
{
    public static class SayHello
    {
        [FunctionName("E1_SayHello")]
        public static string Run([ActivityTrigger] string name)
        {
            return $"Hello {name}!";
        }
    }
}