// pch.h: это предварительно скомпилированный заголовочный файл.
// Перечисленные ниже файлы компилируются только один раз, что ускоряет последующие сборки.
// Это также влияет на работу IntelliSense, включая многие функции просмотра и завершения кода.
// Однако изменение любого из приведенных здесь файлов между операциями сборки приведет к повторной компиляции всех(!) этих файлов.
// Не добавляйте сюда файлы, которые планируете часто изменять, так как в этом случае выигрыша в производительности не будет.

#ifndef PCH_H
#define PCH_H

// Добавьте сюда заголовочные файлы для предварительной компиляции
#include "framework.h"

using namespace std;
using namespace dpp;

#ifdef __cplusplus
extern "C" {
#endif

#ifdef BUILD_DLL
	#define DLL __declspec(dllexport)
#else
	#define DLL __declspec(dllimport)
#endif

	typedef void(__stdcall* Callback_Str)(const char *);
	typedef void(__stdcall* Callback_int32)(const int);

	void DLL SetNotification(Callback_Str notify);
	void DLL mainDLL();
	bool DLL onReady();

	void DLL Get_list_guilds(Callback_Str data);
	void DLL Get_list_DMS(Callback_Str data);
	void DLL Get_list_messages_DM(Callback_Str data, const char* id_user, INT64 before);

	void DLL Send_message_DM(const char* data, const char* id_user);

#ifdef __cplusplus
	}
#endif

#endif //PCH_H
