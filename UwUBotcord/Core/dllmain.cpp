#define _CRT_SECURE_NO_WARNINGS
// dllmain.cpp : Определяет точку входа для приложения DLL.
#include "pch.h"
#define Dedug
// [0] - user
// [1] - moder
// [2] - admin
// [3] - server
#define TypeBulding 2

BOOL APIENTRY DllMain( HMODULE hModule,
                       DWORD  ul_reason_for_call,
                       LPVOID lpReserved
                     )
{
    switch (ul_reason_for_call)
    {
    case DLL_PROCESS_ATTACH:
    case DLL_THREAD_ATTACH:
    case DLL_THREAD_DETACH:
    case DLL_PROCESS_DETACH:
        break;
    }
    return TRUE;
}

//dlL version
string version = "1.0.0";
#if TypeBulding == 0
string type_version = "user";
#elif TypeBulding == 1
string type_version = "moder";
#elif TypeBulding == 2
string type_version = "admin";
#elif TypeBulding == 3
string type_version = "server";
#endif
//socket version
string socket_version = "0.0.0";

#ifdef Dedug
const string    BOT_TOKEN = "";
#else
const string    BOT_TOKEN = "";
#endif
const uint64_t intents_bot = i_default_intents | i_message_content;
cluster bot(BOT_TOKEN, intents_bot);

Callback_Str gNotify;
bool ready = false;

json list_dm_data;

string rgb(int color)
{
	stringstream ss;
	ss << hex << uppercase << setfill('0') << setw(6) << color;
	return ss.str();
}

json Message_to_json(message &m)
{
	json json_message = {
		{"author_id",m.author.id},
		{"author_name", m.author.username},
		{"author_avatar_url", m.author.get_avatar_url(64)},
		{"content", m.content}
	};
	for (int i = 0; i < m.embeds.size(); i++)
	{
		json_message["embeds"][i]["color"] = "#" + rgb(m.embeds[i].color);
		json_message["embeds"][i]["description"] = m.embeds[i].description;
		if (m.embeds[i].thumbnail.has_value())
		{
			json_message["embeds"][i]["thumbnail"] = m.embeds[i].thumbnail.value().url;
		}
		else 
		{
			json_message["embeds"][i]["thumbnail"] = "";
		}
		json_message["embeds"][i]["title"] = m.embeds[i].title;
		if (m.embeds[i].author.has_value())
		{
			json_message["embeds"][i]["author_name"] = m.embeds[i].author.value().name;
			json_message["embeds"][i]["author_icon_url"] = m.embeds[i].author.value().icon_url;
		}
		else
		{
			json_message["embeds"][i]["author_name"] = "";
			json_message["embeds"][i]["author_icon_url"] = "";
		}
		if (m.embeds[i].footer.has_value())
		{
			json_message["embeds"][i]["footer_text"] = m.embeds[i].footer.value().text;
			json_message["embeds"][i]["footer_icon_url"] = m.embeds[i].footer.value().icon_url;
		}
		else
		{
			json_message["embeds"][i]["footer_text"] = "";
			json_message["embeds"][i]["footer_icon_url"] = "";
		}
		if (m.embeds[i].image.has_value())
		{
			json_message["embeds"][i]["image_url"] = m.embeds[i].image.value().url;
		}
		else
		{
			json_message["embeds"][i]["image_url"] = "";
		}
	}
	return json_message;
}

message JsonSTR_to_message(string str_json_message)
{
	json json_message = json::parse(str_json_message);
	message message;
	message.content = json_message["content"];
	return message;
}

json User_to_json(user_identified &user)
{
	string house_hypesquad = "";
	if (user.is_house_balance())
	{
		house_hypesquad = "balance";
	}
	else if (user.is_house_bravery())
	{
		house_hypesquad = "bravery";
	}
	else if (user.is_house_brilliance())
	{
		house_hypesquad = "brilliance";
	}

	json json_user = {
		{"id", user.id},
		{"username", user.username},
		{"discriminator", user.discriminator},
		{"avatar_url",user.get_avatar_url(128)},
		{"banner_url",user.get_banner_url(512)},
		{"is_active_developer",user.is_active_developer()},
		{"is_bot",user.is_bot()},
		{"is_system",user.is_system()},
		{"is_verified",user.is_verified()},
		{"is_discord_employee",user.is_discord_employee()},
		{"is_partnered_owner",user.is_partnered_owner()},
		{"has_hypesquad_events",user.has_hypesquad_events()},
		{"is_bughunter_1",user.is_bughunter_1()},
		{"is_bughunter_2",user.is_bughunter_2()},
		{"hypesquad",house_hypesquad},
		{"is_early_supporter",user.is_early_supporter()},
		{"is_team_user",user.is_team_user()},
		{"is_verified_bot",user.is_verified_bot()},
		{"is_verified_bot_dev",user.is_verified_bot_dev()},
		{"is_certified_moderator",user.is_certified_moderator()},
		{"is_bot_http_interactions",user.is_bot_http_interactions()},
		{"creation_time",to_string(user.get_creation_time())},
		{"accent_color",to_string(user.accent_color)}
	};
	return json_user;
}

bool comp_messages_list(message message_one, message message_two) {
	return message_one.get_creation_time() < message_two.get_creation_time();
}

void Get_list_guilds(Callback_Str data)
{
	if (data != nullptr)
	{
		json json_list_guilds;
		json json_array_id_guilds;

		int index = 0;

		guild_map guild_map = bot.current_user_get_guilds_sync();
		for (const auto& [key, _] : guild_map) {
			guild guild = bot.guild_get_sync(key);
			json_list_guilds["guilds"][to_string(key)] = {
				{"id", to_string(key)},
				{"name", guild.name},
				{"icon_url", guild_map[key].get_icon_url(128)}
			};
			json_array_id_guilds[index] = key;
			index++;
		}

		json_list_guilds["array"] = json_array_id_guilds;

		data(json_list_guilds.dump().c_str());
	}
	else 
	{
		bot.log(loglevel::ll_warning, "Callback of fuction Get_list_guils() is null");
	}
}

void Get_list_DMS(Callback_Str data)
{
	if (data != nullptr)
	{
		json json_list_DMS;
		json json_array_id_user;

		int index = 0;

		for (const auto& [key, value] : list_dm_data.items()) {
			user_identified user = bot.user_get_sync(key);

			json_list_DMS["users"][key] = User_to_json(user);
			json_list_DMS["users"][key]["friend_name"] = value["name"];
			json_list_DMS["users"][key]["accessibility"] = {
				{"access", value["access"]},
				{"type", value["type"]}
			};

			json_array_id_user[index] = key;
			index++;
		}

		json_list_DMS["array"] = json_array_id_user;

		data(json_list_DMS.dump().c_str());
	}
	else
	{
		bot.log(loglevel::ll_warning, "Callback of fuction Get_list_DMS() is null");
	}
}

void Get_list_messages_DM(Callback_Str data, const char* id_user, INT64 before)
{
	string channel_id = list_dm_data[id_user]["channel"].get<string>();

	vector<message> messages_list;

	message_map messages_map = bot.messages_get_sync(channel_id, 0, before, 0, 100);

	for (const auto& [key, _] : messages_map) {
		messages_list.push_back(messages_map[key]);
		cout << key << endl;
	}

	sort(messages_list.begin(), messages_list.end(), comp_messages_list);

	json json_messages_list;

	for (int index = 0; index < messages_list.size(); index++)
	{
		json_messages_list[index] = Message_to_json(messages_list[index]);
	}
	cout << json_messages_list.dump(4) << endl;
	data(json_messages_list.dump().c_str());
}

void Send_message_DM(const char* data, const char* id_user)
{
	//{
	//	{"attachments", [{}, {}, {}]} //{"ephemeral":false, "filename" : "11.txt", "id" : "1077241421964263424", "size" : 3, "url" : "https://cdn.discordapp.com/attachments/695660008964096140/1077241421964263424/11.txt"}
	//	{"channel_id", "11"}
	//	{"content", "<:td_queenfox:924989237487616020 >" }
	//	{"embeds" : [] },
	//	{ "author", {
	//		{"username", "💫✨🍁FuntimeFoxy🍁✨💫"},
	//		{"discriminator", "111"}
	//		{"id", "111"},
	//		{"avatar_url", "111"}
	//	} }
	//	{"is_dm", true}
	//}

	json json_message = json::parse(data);

	message message = JsonSTR_to_message(data);

	message.channel_id = list_dm_data[id_user]["channel"].get<string>();
	bot.message_create_sync(message);

	//int size_attachments = json_message["attachments"].size();
	//int size_embeds = json_message["embeds"].size();

	//if (size_attachments != 0)
	//{
	//	vector<attachment> attachments;
	//	for (int index; index < size_attachments; index++)
	//	{
	//		attachment att(&message);
	//		att.ephemeral = json_message["attachments"][index]["ephemeral"];
	//		att.filename = json_message["attachments"][index]["filename"];
	//		//...//
	//	}
	//}
	//if (size_embeds != 0)
	//{
	//	//...//
	//}


}

void SetNotification(Callback_Str notify)
{
	gNotify = notify;
}

bool onReady()
{
	return ready;
}

void mainDLL() {
	//activity a = new activity();
	ifstream file_list_dm_ifs("list_dm.json");
	if (file_list_dm_ifs.is_open())
	{
		list_dm_data = json::parse(file_list_dm_ifs);
	}

	file_list_dm_ifs.close();

	setlocale(LC_ALL, "en_US.UTF8");

	bot.on_log(utility::cout_logger());

	/*bot.on_slashcommand([](const slashcommand_t & event) {
		if (event.command.get_command_name() == "ping") {
			event.reply("Pong!");
		}
	});*/

	bot.on_ready([&](const ready_t& event) {
		ready = true;

	/*if (run_once<struct register_bot_commands>()) {
		bot.global_command_create(
			dpp::slashcommand("ping", "Ping pong!", bot.me.id)
		);
	}*/
		});
	bot.on_message_create([&](const message_create_t& event) {
		//bot.log(loglevel::ll_info, to_string(event.msg.channel_id) + ": " + event.msg.content);

		if (event.msg.is_dm())
		{
			//test is id to Data_list
			if (!list_dm_data.contains(event.msg.author.id) && event.msg.author.id != bot.current_user_get_sync().id)
			{
				///////////////////////////////////////////////////////////////////// 
				//name of field                               // name              //
				/////////////////////////////////////////////////////////////////////
				//channel is id of channel bot and user       //                   //
				//access is what have the accessibility level //                   //
				// - public is for evetyone                   // {"public"}        //
				// - private is for one user                  //                   //
				//    - [0] only see messgae                  // {"moder private"} //
				//    - [1] nothing                           // {"admin private"} //
				//type is type of user                        //                   //
				// - user                                     // {"user"}          //
				// - admin                                    // {"admin"}         //
				// - moder                                    // {"moder"}         //
				/////////////////////////////////////////////////////////////////////

				list_dm_data[to_string(event.msg.author.id)] = {
					{"channel", event.msg.channel_id},
					{"access", "public"},
					{"name",""},
					{"type","user"}
				};
				bot.log(loglevel::ll_info, "create data of user");
				ofstream file_list_dm_ofs("list_dm.json");
				file_list_dm_ofs << setw(4) << list_dm_data;
			}
			bot.log(loglevel::ll_info, list_dm_data.dump(4));
		}
		if ((int64_t)event.msg.author.id == 609654736731242516) {
			bot.log(loglevel::ll_info, event.msg.build_json());
			if (event.msg.is_dm()) {
				cout << "CI : " << event.msg.channel_id << endl;
			}

			json listDataMsg = {
				{"content", event.msg.content.c_str()}
			};
			cout << "C++ : " << listDataMsg.dump() << endl;
			gNotify(listDataMsg.dump().c_str());
			if (event.msg.content == "~test") {
				bot.message_delete(event.msg.id, event.msg.channel_id);
				message m;
				m.content = "что?";
				m.channel_id = 705417944162893894;
				bot.message_create_sync(m);

				/*channel *channel = find_channel(1035431317829066782);
				command_completion_event_t callback;

				stringstream ss3;

				ss3 << channel->id;

				bot.log(loglevel::ll_debug, ss3.str());

				bot.messages_get(channel->id, 0, 0, 0, 100, [&bot](auto callback) {
					message_map messages = get<message_map>(callback.value);

					stringstream ss, ss2;

					ss << messages.size();

					ss2 << messages[0].build_json();

					bot.log(loglevel::ll_debug, ss.str());

					bot.log(loglevel::ll_debug, ss2.str());
				});*/

				/*bot.messages_get(1035431317829066782, 0, 0, 0, 10, [](const dpp::confirmation_callback_t & event) {
					if (!event.is_error()) {
						auto mm = get<dpp::message_map>(event.value);
						cout << "success; amount of messages:  " << mm.size() << endl;
						cout << "m[0] : " << mm[0].content << endl;
					}
					else {
						cout << "error:  " << event.http_info.body << endl;
					}
				});*/

				/*bot.messages_get(1035431317829066782, 0, 0, 0, 10, [&bot](const dpp::confirmation_callback_t & event) {
					if (!event.is_error()) {
						auto messages = std::get<dpp::message_map>(event.value);
						cout << messages[0].build_json() << endl;
					}
					else {
						bot.log(dpp::ll_error, "couldn't request messages " + event.http_info.body);
					}
				});*/

				//message_map mm = bot.messages_get_sync(705417944162893894llu, 0, 0, 0, 10);

				//cout << "mm.size() : " << mm.size() << endl;

#ifdef  off


				guild g = bot.guild_get_sync(681410931808403513);
				bot.users;

				//user user = bot.user_get_sync(609654736731242516);

				message m;
				m.content = "hey sempai~";

				bot.direct_message_create(user.id, m, [&bot](const dpp::confirmation_callback_t& callback) {
					message callback_message = get<message>(callback.value);
				bot.message_delete(callback_message.id, callback_message.channel_id);
					});
#endif //  off

				//////////////////


				//ids.reserve(mm.size());
				//transform()





				//bot.log(loglevel::ll_debug, ss.str());
				bot.log(loglevel::ll_debug, "hey?");
			}
		}
	//bot.log(loglevel::ll_debug, message);
		});

	bot.start(false);
}