#include "AT_parser.h"

const char ELM327_ID[] PROGMEM = "ELM327 v1.3";
const char FAKE_VOLTAGE[] PROGMEM = "12.6V";
const char FF_mini[] PROGMEM = "FFmini";
const char OBD_header[] PROGMEM = "686AF1";
const uint8_t hex_map[] PROGMEM = {0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46};	// For hex conversion

	
void send_ELM327_prompt()
{
	//Serial_SendByte(0x3E);	//	Sends the '>' prompt
	RingBuffer_Insert(&FFtoBT_Buffer,0x3E);
	send_ELM327_CR();
}
void send_ELM327_OK()
{
	//Serial_SendByte('O');	//
	RingBuffer_Insert(&FFtoBT_Buffer,'O');
	//Serial_SendByte('K');	//
	RingBuffer_Insert(&FFtoBT_Buffer,'K');
	send_ELM327_CR();
}
void send_ELM327_CR()
{
	RingBuffer_Insert(&FFtoBT_Buffer,0x0D);
	//Serial_SendByte(0x0D);	//	CR
}
void send_ELM327_header()
{
	// ADD HEADERS
	Serial_SendString_P(OBD_header);
	//END HEADERS
}



// ****************************************************************************
//		PARSE OBD BT COMMANDS
// ****************************************************************************

void parse_SER_buffer(uint8_t EGT_H, uint8_t EGT_L, uint8_t CHT_H, uint8_t CHT_L, uint32_t total_time_RPM)
{
	uint8_t OBD_headers = 0;
	uint16_t RPM_calc;
	uint8_t temp_ringer;
	uint8_t temp_mode;
	uint16_t tempEGTCHT1;
	uint16_t tempEGTCHT2;

	uint8_t ascii_1;
	uint8_t ascii_2;
	uint8_t ascii_3;
	uint8_t ascii_4;
	
	if( (!(RingBuffer_IsEmpty(&BTtoFF_Buffer))))
	{
		
		temp_ringer = RingBuffer_Remove(&BTtoFF_Buffer);
		
		if(temp_ringer == 'A')
		{
			temp_ringer = RingBuffer_Remove(&BTtoFF_Buffer);
			
			if(temp_ringer == 'T')	// We now have an AT command to parse
			{
				temp_ringer = RingBuffer_Remove(&BTtoFF_Buffer);
				
				if(temp_ringer == 'Z')	// Reset command, fake it
				{
					//Serial_SendString_P(ELM327_ID);
					RingBuffer_Insert(&FFtoBT_Buffer,'E');
					RingBuffer_Insert(&FFtoBT_Buffer,'L');
					RingBuffer_Insert(&FFtoBT_Buffer,'M');
					RingBuffer_Insert(&FFtoBT_Buffer,'3');
					RingBuffer_Insert(&FFtoBT_Buffer,'2');
					RingBuffer_Insert(&FFtoBT_Buffer,'7');
					RingBuffer_Insert(&FFtoBT_Buffer,0X20);	// space
					RingBuffer_Insert(&FFtoBT_Buffer,'v');
					RingBuffer_Insert(&FFtoBT_Buffer,'1');
					RingBuffer_Insert(&FFtoBT_Buffer,'.');
					RingBuffer_Insert(&FFtoBT_Buffer,'3');
					send_ELM327_CR();
					send_ELM327_prompt();
					temp_ringer = RingBuffer_Remove(&BTtoFF_Buffer);	// PULLS CR out of buffer
					CR_flag = 0;	// Clears CR flag
				}
				else if(temp_ringer == 'E')	// Echo Command
				{
					temp_ringer = RingBuffer_Remove(&BTtoFF_Buffer);
					send_ELM327_OK();
					send_ELM327_prompt();
					temp_ringer = RingBuffer_Remove(&BTtoFF_Buffer);	// PULLS CR out of buffer
					CR_flag = 0;	// Clears CR flag
				
				}
				else if(temp_ringer == 'M')	// Protocol Memory Command
				{
					temp_ringer = RingBuffer_Remove(&BTtoFF_Buffer);
					send_ELM327_OK();
					send_ELM327_prompt();
					temp_ringer = RingBuffer_Remove(&BTtoFF_Buffer);	// PULLS CR out of buffer
					CR_flag = 0;	// Clears CR flag
				
				}
				else if(temp_ringer == 'L')	// Line Feed Command
				{
					temp_ringer = RingBuffer_Remove(&BTtoFF_Buffer);
					send_ELM327_OK();
					send_ELM327_prompt();
					temp_ringer = RingBuffer_Remove(&BTtoFF_Buffer);	// PULLS CR out of buffer
					CR_flag = 0;	// Clears CR flag
				
				}
				else if(temp_ringer == 'S')	// Blank Spaces or Store Protocol Command
				{
					temp_ringer = RingBuffer_Remove(&BTtoFF_Buffer);
					if(temp_ringer == 'H')	// Set Header
					{
						temp_ringer = RingBuffer_Remove(&BTtoFF_Buffer);
						temp_ringer = RingBuffer_Remove(&BTtoFF_Buffer);
						temp_ringer = RingBuffer_Remove(&BTtoFF_Buffer);
						temp_ringer = RingBuffer_Remove(&BTtoFF_Buffer);
						temp_ringer = RingBuffer_Remove(&BTtoFF_Buffer);
						temp_ringer = RingBuffer_Remove(&BTtoFF_Buffer);
					}
					else if(temp_ringer == 'P')	// Set Protocol
					{
						temp_ringer = RingBuffer_Remove(&BTtoFF_Buffer);	// 0 or 1
					}
					
					send_ELM327_OK();
					send_ELM327_prompt();
					temp_ringer = RingBuffer_Remove(&BTtoFF_Buffer);	// PULLS CR out of buffer		
					CR_flag = 0;	// Clears CR flag
				
				}
				else if(temp_ringer == 'H')	// Headers Command
				{
					temp_ringer = RingBuffer_Remove(&BTtoFF_Buffer);
					//SendSERBuffer[temp_USB_indexer++]  = temp_ringer;
					if(temp_ringer == '1')
					{
						OBD_headers = 1;
					}
					else
					{
						OBD_headers = 0;
					}
					send_ELM327_OK();
					send_ELM327_prompt();
					temp_ringer = RingBuffer_Remove(&BTtoFF_Buffer);	// PULLS CR out of buffer
					CR_flag = 0;	// Clears CR flag
				
				}
				
				else if(temp_ringer == 'R')	// Responses Command
				{
					temp_ringer = RingBuffer_Remove(&BTtoFF_Buffer);
					//SendSERBuffer[temp_USB_indexer++]  = temp_ringer;
					if(temp_ringer == 'V')
					{
						//Serial_SendString_P(FAKE_VOLTAGE);
						RingBuffer_Insert(&FFtoBT_Buffer,'1');
						RingBuffer_Insert(&FFtoBT_Buffer,'2');
						RingBuffer_Insert(&FFtoBT_Buffer,'.');
						RingBuffer_Insert(&FFtoBT_Buffer,'6');
						RingBuffer_Insert(&FFtoBT_Buffer,'V');
						send_ELM327_CR();
						send_ELM327_prompt();
					}
					else
					{					
						send_ELM327_OK();
						send_ELM327_prompt();
					}
					temp_ringer = RingBuffer_Remove(&BTtoFF_Buffer);	// PULLS CR out of buffer
					CR_flag = 0;	// Clears CR flag
				
				}
				else if(temp_ringer == 'V')	// Variable DLC Command
				{
					temp_ringer = RingBuffer_Remove(&BTtoFF_Buffer);
					//SendSERBuffer[temp_USB_indexer++]  = temp_ringer;
					send_ELM327_OK();
					send_ELM327_prompt();
					temp_ringer = RingBuffer_Remove(&BTtoFF_Buffer);	// PULLS CR out of buffer
					CR_flag = 0;	// Clears CR flag
					
				}
				else if(temp_ringer == '1')	// ATAT1 Adaptive headers Command
				{
					send_ELM327_OK();
					send_ELM327_prompt();
					temp_ringer = RingBuffer_Remove(&BTtoFF_Buffer);	// PULLS CR out of buffer
					CR_flag = 0;	// Clears CR flagg
				
				}
				else if(temp_ringer == '@')	// Blank Spaces Command
				{
					temp_ringer = RingBuffer_Remove(&BTtoFF_Buffer);
					//SendSERBuffer[temp_USB_indexer++]  = temp_ringer;
					if(temp_ringer == '1')	// Descriptor
					{
						//Serial_SendString_P(FF_mini);
						RingBuffer_Insert(&FFtoBT_Buffer,'F');
						RingBuffer_Insert(&FFtoBT_Buffer,'F');
						RingBuffer_Insert(&FFtoBT_Buffer,'m');
						RingBuffer_Insert(&FFtoBT_Buffer,'i');
						RingBuffer_Insert(&FFtoBT_Buffer,'n');
						RingBuffer_Insert(&FFtoBT_Buffer,'i');
						send_ELM327_CR();
						send_ELM327_prompt();
						temp_ringer = RingBuffer_Remove(&BTtoFF_Buffer);	// PULLS CR out of buffer
						CR_flag = 0;	// Clears CR flag
					}
					else
					{
						send_ELM327_OK();
						send_ELM327_prompt();
						temp_ringer = RingBuffer_Remove(&BTtoFF_Buffer);	// PULLS CR out of buffer
						CR_flag = 0;	// Clears CR flag
					}
				
				}
				else if(temp_ringer == 'I')	// ID Yourself command
				{
					//Serial_SendString_P(ELM327_ID);
					RingBuffer_Insert(&FFtoBT_Buffer,'E');
					RingBuffer_Insert(&FFtoBT_Buffer,'L');
					RingBuffer_Insert(&FFtoBT_Buffer,'M');
					RingBuffer_Insert(&FFtoBT_Buffer,'3');
					RingBuffer_Insert(&FFtoBT_Buffer,'2');
					RingBuffer_Insert(&FFtoBT_Buffer,'7');
					RingBuffer_Insert(&FFtoBT_Buffer,0X20);	// space
					RingBuffer_Insert(&FFtoBT_Buffer,'v');
					RingBuffer_Insert(&FFtoBT_Buffer,'1');
					RingBuffer_Insert(&FFtoBT_Buffer,'.');
					RingBuffer_Insert(&FFtoBT_Buffer,'3');
					send_ELM327_CR();
					send_ELM327_prompt();
					temp_ringer = RingBuffer_Remove(&BTtoFF_Buffer);	// PULLS CR out of buffer
					CR_flag = 0;	// Clears CR flag
				
				}
			
			}
		}
	//***********************************************************
	//	Didnt find AT command, so look for OBD command
	//***********************************************************
		else if (temp_ringer == '0')
		{
			temp_mode = RingBuffer_Remove(&BTtoFF_Buffer);
			//SendSERBuffer[temp_USB_indexer++]  = temp_mode;
			switch (temp_mode)
			{
				case '1':	// Mode 01
				temp_ringer = RingBuffer_Remove(&BTtoFF_Buffer);
				//SendSERBuffer[temp_USB_indexer++]  = temp_ringer;
				if ((RingBuffer_Peek(&BTtoFF_Buffer)== '0'))
				{
					RingBuffer_Remove(&BTtoFF_Buffer);	// Removes peeked value above
					//SendSERBuffer[temp_USB_indexer++]  = '0';
					if(temp_ringer == '0')	// What PIDS are supported	0100
					{
						//Serial_SendByte('4');
						//Serial_SendByte('1');
						RingBuffer_Insert(&FFtoBT_Buffer,'4');
						RingBuffer_Insert(&FFtoBT_Buffer,'1');
						
						if(OBD_headers)
						{
							send_ELM327_header();		
						}
						//Serial_SendByte('0');
						//Serial_SendByte('0');
						RingBuffer_Insert(&FFtoBT_Buffer,'0');
						RingBuffer_Insert(&FFtoBT_Buffer,'0');
						// Supported PIDS below
						//Serial_SendByte('0');
						//Serial_SendByte('0');
						//Serial_SendByte('1');	// Support for RPM PID 0x0C
						//Serial_SendByte('0');
						//Serial_SendByte('0');
						//Serial_SendByte('0');
						//Serial_SendByte('0');
						//Serial_SendByte('0');
						RingBuffer_Insert(&FFtoBT_Buffer,'0');
						RingBuffer_Insert(&FFtoBT_Buffer,'0');
						RingBuffer_Insert(&FFtoBT_Buffer,'1');
						RingBuffer_Insert(&FFtoBT_Buffer,'0');
						RingBuffer_Insert(&FFtoBT_Buffer,'0');
						RingBuffer_Insert(&FFtoBT_Buffer,'0');
						RingBuffer_Insert(&FFtoBT_Buffer,'0');
						RingBuffer_Insert(&FFtoBT_Buffer,'0');
						send_ELM327_CR();
						send_ELM327_prompt();
						temp_ringer = RingBuffer_Remove(&BTtoFF_Buffer);	// PULLS CR out of buffer
						CR_flag = 0;	// Clears CR flag
					}
					else if (temp_ringer == '2')	// 0120, PIDS 21-40 supported bit mask
					{
						//Serial_SendByte('4');
						//Serial_SendByte('1');
						//Serial_SendByte('2');
						//Serial_SendByte('0');
						RingBuffer_Insert(&FFtoBT_Buffer,'4');
						RingBuffer_Insert(&FFtoBT_Buffer,'1');
						RingBuffer_Insert(&FFtoBT_Buffer,'2');
						RingBuffer_Insert(&FFtoBT_Buffer,'0');						
						// Supported PIDS below
						//Serial_SendByte('0');
						//Serial_SendByte('0');
						//Serial_SendByte('0');
						//Serial_SendByte('0');
						//Serial_SendByte('0');
						//Serial_SendByte('0');	// Support for EGT bank PID 0x78
						//Serial_SendByte('1');
						//Serial_SendByte('8');
						RingBuffer_Insert(&FFtoBT_Buffer,'0');
						RingBuffer_Insert(&FFtoBT_Buffer,'0');
						RingBuffer_Insert(&FFtoBT_Buffer,'0');
						RingBuffer_Insert(&FFtoBT_Buffer,'0');
						RingBuffer_Insert(&FFtoBT_Buffer,'0');
						RingBuffer_Insert(&FFtoBT_Buffer,'0');
						RingBuffer_Insert(&FFtoBT_Buffer,'1');
						RingBuffer_Insert(&FFtoBT_Buffer,'8');
						send_ELM327_CR();
						send_ELM327_prompt();
						temp_ringer = RingBuffer_Remove(&BTtoFF_Buffer);	// PULLS CR out of buffer
						CR_flag = 0;	// Clears CR flag
					}
				
					else    // kind of assuming its asking for what we support
					{
						//Serial_SendByte('4');
						//Serial_SendByte('1');
						//Serial_SendByte(temp_ringer);
						//Serial_SendByte('0');
						RingBuffer_Insert(&FFtoBT_Buffer,'4');
						RingBuffer_Insert(&FFtoBT_Buffer,'1');
						RingBuffer_Insert(&FFtoBT_Buffer,temp_ringer);
						RingBuffer_Insert(&FFtoBT_Buffer,'0');
						// Supported PIDS below
						//Serial_SendByte('0');
						//Serial_SendByte('0');
						//Serial_SendByte('0');
						//Serial_SendByte('0');
						//Serial_SendByte('0');
						//Serial_SendByte('0');
						//Serial_SendByte('0');
						//Serial_SendByte('0');
						RingBuffer_Insert(&FFtoBT_Buffer,'0');
						RingBuffer_Insert(&FFtoBT_Buffer,'0');
						RingBuffer_Insert(&FFtoBT_Buffer,'0');
						RingBuffer_Insert(&FFtoBT_Buffer,'0');
						RingBuffer_Insert(&FFtoBT_Buffer,'0');
						RingBuffer_Insert(&FFtoBT_Buffer,'0');
						RingBuffer_Insert(&FFtoBT_Buffer,'0');
						RingBuffer_Insert(&FFtoBT_Buffer,'0');
						send_ELM327_CR();	//	CR
						send_ELM327_prompt();
						temp_ringer = RingBuffer_Remove(&BTtoFF_Buffer);	// PULLS CR out of buffer
						CR_flag = 0;	// Clears CR flag
					}
				
				}
			
				else if (temp_ringer == '0')	// 010x
				{
					temp_ringer = RingBuffer_Remove(&BTtoFF_Buffer);
					
					if(temp_ringer == 'C')		// 010C
					{
						// RPM request
						//if((!first_cycle)&&(total_time_RPM))
						if(total_time_RPM)
						{
							//	This takes the total_time = timer1_OVF + timer1_HIGH + timer1_LOW
							//	and converts to the format required for OBD msg
							//	AND IT NEEDS TO BE OPTIMIZED TOO LARGE!!
							RPM_calc = (uint16_t)(((0xE4E1C000)/total_time_RPM));	
							ascii_4 =  pgm_read_byte(&hex_map[RPM_calc&0x000F]);
							RPM_calc >>=0x04;
							ascii_3 =  pgm_read_byte(&hex_map[RPM_calc&0x000F]);
							RPM_calc >>=0x04;
							ascii_2 =  pgm_read_byte(&hex_map[RPM_calc&0x000F]);
							RPM_calc >>=0x04;
							ascii_1 =  pgm_read_byte(&hex_map[RPM_calc&0x000F]);
						}
						else
						{
							// IDLE
							ascii_1 = '0';
							ascii_2 = '0';
							ascii_3 = '0';
							ascii_4 = '0';
						}
						//Serial_SendByte('4');
						//Serial_SendByte('1');
						//Serial_SendByte('0');
						//Serial_SendByte('C');
						RingBuffer_Insert(&FFtoBT_Buffer,'4');
						RingBuffer_Insert(&FFtoBT_Buffer,'1');
						RingBuffer_Insert(&FFtoBT_Buffer,'0');
						RingBuffer_Insert(&FFtoBT_Buffer,'C');
						//Serial_SendByte(ascii_1);
						//Serial_SendByte(ascii_2);
						//Serial_SendByte(ascii_3);
						//Serial_SendByte(ascii_4);
						RingBuffer_Insert(&FFtoBT_Buffer,ascii_1);
						RingBuffer_Insert(&FFtoBT_Buffer,ascii_2);
						RingBuffer_Insert(&FFtoBT_Buffer,ascii_3);
						RingBuffer_Insert(&FFtoBT_Buffer,ascii_4);
						send_ELM327_CR();
						send_ELM327_prompt();
						temp_ringer = RingBuffer_Remove(&BTtoFF_Buffer);	// PULLS CR out of buffer
						CR_flag = 0;	// Clears CR flag
					}
				}
				else if (temp_ringer == '3')
				{
					temp_ringer = RingBuffer_Remove(&BTtoFF_Buffer);
					//SendSERBuffer[temp_USB_indexer++]  = temp_ringer;
					if(temp_ringer == 'C')
					{
						// EGT request
						if(EGT_L & 0x04)
						{
							//	OPEN SENSOR
							//	All 0's is actually -40 in OBD 
							ascii_1 = '0';
							ascii_2 = '0';
							ascii_3 = '0';
							ascii_4 = '0';
						}
						else
						{
							//	This convert raw EGT and CHT into OBD format
							tempEGTCHT1 = EGT_H;
							tempEGTCHT1 = tempEGTCHT1<<8;
							tempEGTCHT1 = tempEGTCHT1 + EGT_L;
							tempEGTCHT1 &= 0x7FF8;
							tempEGTCHT1 >>= 0x03;
							tempEGTCHT2 = tempEGTCHT1;
							tempEGTCHT1 = tempEGTCHT1<<1;
							tempEGTCHT2 = tempEGTCHT2>>1;
							tempEGTCHT1 = tempEGTCHT1 + tempEGTCHT2;
							tempEGTCHT1 = tempEGTCHT1 + 400;
							ascii_4 =  pgm_read_byte(&hex_map[tempEGTCHT1&0x000F]);
							tempEGTCHT1 >>=0x04;
							ascii_3 =  pgm_read_byte(&hex_map[tempEGTCHT1&0x000F]);
							tempEGTCHT1 >>=0x04;
							ascii_2 =  pgm_read_byte(&hex_map[tempEGTCHT1&0x000F]);
							tempEGTCHT1 >>=0x04;
							ascii_1 =  pgm_read_byte(&hex_map[tempEGTCHT1&0x000F]);
						}
						//Serial_SendByte('4');
						//Serial_SendByte('1');
						//Serial_SendByte('3');
						//Serial_SendByte('C');
						RingBuffer_Insert(&FFtoBT_Buffer,'4');
						RingBuffer_Insert(&FFtoBT_Buffer,'1');
						RingBuffer_Insert(&FFtoBT_Buffer,'3');
						RingBuffer_Insert(&FFtoBT_Buffer,'C');
						//Serial_SendByte(ascii_1);
						//Serial_SendByte(ascii_2);
						//Serial_SendByte(ascii_3);
						//Serial_SendByte(ascii_4);
						RingBuffer_Insert(&FFtoBT_Buffer,ascii_1);
						RingBuffer_Insert(&FFtoBT_Buffer,ascii_2);
						RingBuffer_Insert(&FFtoBT_Buffer,ascii_3);
						RingBuffer_Insert(&FFtoBT_Buffer,ascii_4);
						send_ELM327_CR();
						send_ELM327_prompt();
						temp_ringer = RingBuffer_Remove(&BTtoFF_Buffer);	// PULLS CR out of buffer
						CR_flag = 0;	// Clears CR flag
					
					}
					else if(temp_ringer == 'D')
					{
						// CHT request
						if(CHT_L & 0x04)
						{
							//	OPEN SENSOR
							//	All 0's is actually -40 in OBD 
							ascii_1 = '0';
							ascii_2 = '0';
							ascii_3 = '0';
							ascii_4 = '0';
						}
						else
						{
							//	This convert raw EGT and CHT into OBD format
							tempEGTCHT1 = CHT_H;
							tempEGTCHT1 = tempEGTCHT1<<8;
							tempEGTCHT1 = tempEGTCHT1 + CHT_L;
							tempEGTCHT1 &= 0x7FF8;
							tempEGTCHT1 >>= 0x03;
							tempEGTCHT2 = tempEGTCHT1;
							tempEGTCHT1 = tempEGTCHT1<<1;
							tempEGTCHT2 = tempEGTCHT2>>1;
							tempEGTCHT1 = tempEGTCHT1 + tempEGTCHT2;
							tempEGTCHT1 = tempEGTCHT1 + 400;
							ascii_4 =  pgm_read_byte(&hex_map[tempEGTCHT1&0x000F]);
							tempEGTCHT1 >>=0x04;
							ascii_3 =  pgm_read_byte(&hex_map[tempEGTCHT1&0x000F]);
							tempEGTCHT1 >>=0x04;
							ascii_2 =  pgm_read_byte(&hex_map[tempEGTCHT1&0x000F]);
							tempEGTCHT1 >>=0x04;
							ascii_1 =  pgm_read_byte(&hex_map[tempEGTCHT1&0x000F]);
						}
						//Serial_SendByte('4');
						//Serial_SendByte('1');
						//Serial_SendByte('3');
						//Serial_SendByte('D');
						//Sensor #2 value
						RingBuffer_Insert(&FFtoBT_Buffer,'4');
						RingBuffer_Insert(&FFtoBT_Buffer,'1');
						RingBuffer_Insert(&FFtoBT_Buffer,'3');
						RingBuffer_Insert(&FFtoBT_Buffer,'D');
						//Serial_SendByte(ascii_1);
						//Serial_SendByte(ascii_2);
						//Serial_SendByte(ascii_3);
						//Serial_SendByte(ascii_4);
						RingBuffer_Insert(&FFtoBT_Buffer,ascii_1);
						RingBuffer_Insert(&FFtoBT_Buffer,ascii_2);
						RingBuffer_Insert(&FFtoBT_Buffer,ascii_3);
						RingBuffer_Insert(&FFtoBT_Buffer,ascii_4);
						send_ELM327_CR();
						send_ELM327_prompt();
						temp_ringer = RingBuffer_Remove(&BTtoFF_Buffer);	// PULLS CR out of buffer
						CR_flag = 0;	// Clears CR flag
					
					}
				}
			
				break;
				case '2':
				case '3':
				case '4':
				case '5':
				case '6':
				case '7':
				case '8':
				case '9':
				case 'A':
				//	We dont support these so respond accordingly
				//Serial_SendByte('4');
				RingBuffer_Insert(&FFtoBT_Buffer,'4');
				//Serial_SendByte(temp_mode);
				RingBuffer_Insert(&FFtoBT_Buffer,temp_mode);
				//Serial_SendByte(RingBuffer_Remove(&BTtoFF_Buffer));
				RingBuffer_Insert(&FFtoBT_Buffer,RingBuffer_Remove(&BTtoFF_Buffer));
				//Serial_SendByte(RingBuffer_Remove(&BTtoFF_Buffer));
				RingBuffer_Insert(&FFtoBT_Buffer,RingBuffer_Remove(&BTtoFF_Buffer));
				send_ELM327_CR();
				send_ELM327_prompt();
				temp_ringer = RingBuffer_Remove(&BTtoFF_Buffer);	// PULLS CR out of buffer
				CR_flag = 0;	// Clears CR flag
				break;
			}
		}
	}
	
	

}	// END PARSE_SER_BUFFER


