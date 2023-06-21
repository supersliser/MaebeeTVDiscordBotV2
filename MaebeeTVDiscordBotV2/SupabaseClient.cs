using Supabase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class SupabaseClient
{
    protected Client client;
    public SupabaseClient()
    {
        var options = new Supabase.SupabaseOptions
        {
            AutoConnectRealtime = true
        };

        client = new Supabase.Client(@"https://zplxxxsyqqrieohussiw.supabase.co", @"eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6InpwbHh4eHN5cXFyaWVvaHVzc2l3Iiwicm9sZSI6InNlcnZpY2Vfcm9sZSIsImlhdCI6MTY4NjE1MzY4MSwiZXhwIjoyMDAxNzI5NjgxfQ.iJDvwnv-lQAU2xAYNjeo_YZS3c_GdChVfIim1Ty4s5c", options);
        client.InitializeAsync();
    }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
    public virtual async Task PushToDatabase()
    {

    }
    public virtual async Task<supabaseTeam> GetFromDatabase()
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
    {
        return null;
    }
}
