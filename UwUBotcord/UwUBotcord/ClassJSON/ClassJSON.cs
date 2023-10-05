using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace UwUBotcord.ClassJSON
{
    internal class Guilds
    {
        public string id { get; set; }
        public string name { get; set; }
        public string icon_url { get; set; }
    };
    internal class List_guilds
    {
        public Dictionary<string, Guilds> guilds { get; set; }
        public string[] array { get; set; }
    };
    internal class List_DMS
    {
        public string username { get; set; }
        public string discriminator { get; set; }
        public string avatar_url { get; set; }
        public string banner_url { get; set; }
        public string hypesquad { get; set; }
        public string creation_time { get; set; }
        public string accent_color { get; set; }
        public bool is_active_developer { get; set; }
        public bool is_bot { get; set; }
        public bool is_system { get; set; }
        public bool is_verified { get; set; }
        public bool is_discord_employee { get; set; }
        public bool is_partnered_owner { get; set; }
        public bool has_hypesquad_events { get; set; }
        public bool is_bughunter_1 { get; set; }
        public bool is_bughunter_2 { get; set; }
        public bool is_early_supporter { get; set; }
        public bool is_team_user { get; set; }
        public bool is_verified_bot { get; set; }
        public bool is_verified_bot_dev { get; set; }
        public bool is_certified_moderator { get; set; }
        public bool is_bot_http_interactions { get; set; }
        public DM_user_stats stats { get; set; }

    };
    internal class DM_user_stats
    {
        public string access { get; set; }
        public string type { get; set; }
    };
    internal class Message
    {
        public string author_id { get; set; }
        public string content { get; set; }
        public string author_name { get; set; }
        public string author_avatar_url { get; set; }

        public List<Embed> embeds { get; set; }
    };
    internal class Embed
    {
        //public Embed ()
        //{
        //    this.author_name = "??";
        //}
        public string color { get; set; }
        public string description { get; set; }
        public string thumbnail { get; set; }
        public string title { get; set; }
        public string author_name { get; set; }
        public string author_icon_url { get; set; }
        public string footer_text { get; set; }
        public string footer_icon_url { get; set; }
        public string image_url { get; set; }
    };
}
