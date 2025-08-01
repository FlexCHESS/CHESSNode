/************************************************************************************************
    SBFspot - Yet another tool to read power production of SMA solar inverters
    (c)2012-2022, SBF

    Latest version found at https://github.com/SBFspot/SBFspot

    License: Attribution-NonCommercial-ShareAlike 3.0 Unported (CC BY-NC-SA 3.0)
    http://creativecommons.org/licenses/by-nc-sa/3.0/

    You are free:
        to Share - to copy, distribute and transmit the work
        to Remix - to adapt the work
    Under the following conditions:
    Attribution:
        You must attribute the work in the manner specified by the author or licensor
        (but not in any way that suggests that they endorse you or your use of the work).
    Noncommercial:
        You may not use this work for commercial purposes.
    Share Alike:
        If you alter, transform, or build upon this work, you may distribute the resulting work
        only under the same or similar license to this one.

DISCLAIMER:
    A user of SBFspot software acknowledges that he or she is receiving this
    software on an "as is" basis and the user is not relying on the accuracy
    or functionality of the software for any purpose. The user further
    acknowledges that any use of this software will be at his own risk
    and the copyright owner accepts no responsibility whatsoever arising from
    the use or application of the software.

    SMA is a registered trademark of SMA Solar Technology AG

************************************************************************************************/

#pragma once

#include "osselect.h"

#if defined(_WIN32)

// Ignore warning C4127: conditional expression is constant
#pragma warning(disable: 4127)

#include <WinSock2.h>
#include <ws2tcpip.h>

//Windows Sockets Error Codes
//http://msdn.microsoft.com/en-us/library/ms740668(v=vs.85).aspx

#endif  /* _WIN32 */

#if defined(__linux__)
#include <sys/select.h>
#include <sys/socket.h>
#include <ifaddrs.h>
#include <net/if.h>
#include <netdb.h>
#include <netinet/in.h>
#include <arpa/inet.h>
#include <unistd.h>
#include <fcntl.h>
#include <sys/ioctl.h>
#include <errno.h>
#include <string.h>

#endif	/* __linux__ */

#include <stdio.h>
#include <ctype.h>
#include <iostream>

uint8_t char2dec(char ch);
uint8_t hexbyte2dec(char *hex);

#define BT_NUMRETRY 10
#define BT_TIMEOUT  10

extern int packetposition;
extern int MAX_CommBuf;

extern int debug;
extern int verbose;

extern SOCKET sock;
extern struct sockaddr_in addr_in, addr_out;

//Function prototypes
int ethConnect(short port);
int ethClose(void);
int getLocalIP(uint8_t IPAddress[4]);
int ethSend(uint8_t *buffer, const char *toIP);
int ethRead(uint8_t *buf, unsigned int bufsize);
