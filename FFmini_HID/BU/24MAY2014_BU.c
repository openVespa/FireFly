/*
             LUFA Library
     Copyright (C) Dean Camera, 2014.

  dean [at] fourwalledcubicle [dot] com
           www.lufa-lib.org
*/

/*
  Copyright 2014  Dean Camera (dean [at] fourwalledcubicle [dot] com)

  Permission to use, copy, modify, distribute, and sell this
  software and its documentation for any purpose is hereby granted
  without fee, provided that the above copyright notice appear in
  all copies and that both that the copyright notice and this
  permission notice and warranty disclaimer appear in supporting
  documentation, and that the name of the author not be used in
  advertising or publicity pertaining to distribution of the
  software without specific, written prior permission.

  The author disclaims all warranties with regard to this
  software, including all implied warranties of merchantability
  and fitness.  In no event shall the author be liable for any
  special, indirect or consequential damages or any damages
  whatsoever resulting from loss of use, data or profits, whether
  in an action of contract, negligence or other tortious action,
  arising out of or in connection with the use or performance of
  this software.
*/

/** \file
 *
 *  Main source file for the GenericHID demo. This file contains the main tasks of
 *  the demo and is responsible for the initial application hardware configuration.
 */

#include "GenericHID.h"
#include "map1.h"
#include "map2.h"
//#include "map3.h"	// This was just a debug test map

volatile uint8_t USBConnected;
volatile uint8_t first_cycle;
volatile uint8_t bad_count;
volatile uint8_t send_flag;
volatile uint8_t map_num;
volatile uint8_t request_num;
volatile uint8_t seq_num;
volatile uint8_t MAP_STATIC_TEMP;
volatile uint8_t temp_rset;

// TO DO: combine all of these into one byte
volatile uint8_t ISR_flags;
volatile uint8_t reluctor_trigger;
volatile uint8_t t0OVF_trigger;
volatile uint8_t serial_trigger;
volatile uint8_t t1OVF_trigger;

volatile uint8_t timer1_OVF_RPM;
volatile uint8_t timer1H;
volatile uint8_t timer1L;
volatile uint8_t tcA_high;
volatile uint8_t tcA_low;
volatile uint8_t tcB_high;
volatile uint8_t tcB_low;

volatile uint8_t cht_h = 0;
volatile uint8_t cht_l = 0;
volatile uint8_t egt_h = 0;
volatile uint8_t egt_l = 0;

/** Buffer to hold the previously generated HID report, for comparison purposes inside the HID class driver. */
static uint8_t PrevHIDReportBuffer[GENERIC_REPORT_SIZE];

/** LUFA HID Class driver interface configuration and state information. This structure is
 *  passed to all HID Class driver functions, so that multiple instances of the same class
 *  within a device can be differentiated from one another.
 */
USB_ClassInfo_HID_Device_t Generic_HID_Interface =
	{
		.Config =
			{
				.InterfaceNumber              = INTERFACE_ID_GenericHID,
				.ReportINEndpoint             =
					{
						.Address              = GENERIC_IN_EPADDR,
						.Size                 = GENERIC_EPSIZE,
						.Banks                = 1,
					},
				.PrevReportINBuffer           = PrevHIDReportBuffer,
				.PrevReportINBufferSize       = sizeof(PrevHIDReportBuffer),
			},
	};


/** Main program entry point. This routine contains the overall program flow, including initial
 *  setup of all components and the main program loop.
 */
int main(void)
{
	//SetupHardware();

	//LEDs_SetAllLEDs(LEDMASK_USB_NOTREADY);
	//GlobalInterruptEnable();
		
	temp_rset = MCUSR;	// Grab reset register
	MCUSR = 0x00;		// Clear reset register
	
	/* Disable watchdog if enabled by boot loader/fuses */
	MCUSR &= ~(1 << WDRF);
	wdt_disable();
	
	static uint16_t MAP[101];
		
	uint16_t tempdiv = 0;
	uint32_t total_time = 0;
	uint8_t temp_high = 0;
	uint8_t temp_low = 0;
	uint8_t RPM;
	//uint8_t OLD_RPM1;
	//uint8_t OLD_RPM2;
	//uint8_t OLD_RPM3;
	//uint8_t OLD_RPM4;
	uint8_t OVFt1;
	uint8_t OVFt1OLD;
	//uint8_t MA;
	//uint8_t MAL;
	//uint8_t MAH;
	
	
	//uint8_t serial_msg_count = 0;
	//uint16_t crc;
	uint8_t i;	// generic indexer
	
	//uint8_t FF_FWversion = 1;
	uint8_t cycle_count = 0;
	
	uint8_t EGT_check_flag = 0;	
	
	
		OVFt1 = 0;					// Reset the OVF counter
		OVFt1OLD = 0;				// Reset the OVF counter
		
		// Start with CDI trigger low
		PORTD &= ~(_BV(IGN_TRIGGER));	//  LOW
				
		first_cycle = 1;
		send_flag = 0;
		bad_count = 0;
		RPM = 0;
		//OLD_RPM1 = 9;
		//OLD_RPM2 = 9;
		//OLD_RPM3 = 9;
		//OLD_RPM4 = 9;
		timer1H = 0;
		timer1L = 0;
		ISR_flags = 0;
		reluctor_trigger = 0;
		timer1_OVF_RPM = 0;
		t0OVF_trigger = 0;
		t1OVF_trigger = 0;
		tcA_high = 0;
		tcA_low = 0;
		tcB_high = 0;
		tcB_low = 0;
		//crc = 0xffff;	// initialize crc
		
		CR_flag = 0;
		// Starter MA values allows first cycle through
		//MA = 0;
		//MAL = 0;
		//MAH = 101;


		// 0 - INPUT
		// 1 - OUTPUT
		
		DDRB = 0x10; 	//
		DDRC = 0x20;	//
		DDRD = 0x41;    //

	//**********************************************************
	SetupHardware();
	RingBuffer_InitBuffer(&BTtoFF_Buffer, BTtoFF_Buffer_Data, sizeof(BTtoFF_Buffer_Data)); 
	
	//**********************************************************
	// FF PULL
	// TO DO: Switch to the LUFA switch reads?
	if((PINB & 0x20))
	{// && (!(PINF & 0x40))){	// LOAD MAP 2
		map_num = 0x02;	// Indicates MAP 2 is loaded
		eeprom_busy_wait();
		MAP_STATIC_TEMP = eeprom_read_byte((uint8_t*)&MAP2_STATIC);
		for(i = 0;i<101;i++)
		{
			eeprom_busy_wait();
			MAP[i] = eeprom_read_word((uint16_t*)&MAP2[i]);
		}
	}
	
	else 
	{	// LOAD MAP 1
		map_num = 0x01;	// Indicates MAP 1 is loaded
		eeprom_busy_wait();
		MAP_STATIC_TEMP = eeprom_read_byte((uint8_t*)&MAP1_STATIC);
		for(i = 0;i<101;i++)
		{
			eeprom_busy_wait();
			MAP[i] = eeprom_read_word((uint16_t*)&MAP1[i]);
		}		
		
	}
	
	TCNT1 = 0;		// Reset Timer1
	
	//LEDs_TurnOffLEDs(LEDS_LED1);
	PORTD &= ~(_BV(MAIN_LED));	//  LOW 
	request_num = 0x01;	// Gets us sending back data packets
	sei();	// Enable interrupts


	for (;;)
	{
		HID_Device_USBTask(&Generic_HID_Interface);
		USB_USBTask();
		
		// Check the flags
		cli();
		//********************************************************************
		// 	IF this timer ever overflows more than 146 times,
		//  the engine is running at less than 100 RPMs.
		if(timer1_OVF_RPM > 146) 
		{	
			// Disable comparators
			TIMSK1&=~(1<<OCIE1A); 				   	// Disable timer1A compare
			TIMSK1&=~(1<<OCIE1B); 				   	// Disable timer1B compare
			first_cycle = 1;
			total_time = 0;
			OVFt1 = 0;					// Reset the OVF counter
			timer1_OVF_RPM = 0;
			cycle_count = 0;
			//OLD_RPM1 = 9;
			//OLD_RPM2 = 9;
			//OLD_RPM3 = 9;
			//OLD_RPM4 = 9;
			//MAL = 0;
			//MAH = 101;
			// Enable the pass thru circuit
			PORTD &= ~(_BV(IGN_TRIGGER));	//  LOW 
		}	
//********************************************************************
		if (reluctor_trigger) // time to make the donuts
		{
			if (!first_cycle)
			{
				PORTD &= ~(_BV(IGN_TRIGGER));		//  LOW 

				// Copy volatile variables to local copies
				// Does this help? Verify?
				
				temp_high = timer1H;
				temp_low  = timer1L;
				OVFt1 = timer1_OVF_RPM;
					
				if( (OVFt1 > 0) )
				{
					tempdiv = 0x00FF & OVFt1;
					tempdiv = tempdiv<<8;
					tempdiv = tempdiv + temp_high;
					RPM = (uint8_t)(37528/tempdiv);
					total_time = tempdiv;
					total_time = total_time<<8;
					total_time = total_time + temp_low;

					if(RPM < 101) 
					{
						TCNT1 = 0x0000;		// Reset Timer1
						OCR1A = MAP[RPM];
						OCR1B = MAP[RPM] + 800;	// To Do: Change this to a variable
																				
						// Enable CompA, Disable CompB
						TIFR1 = (1<<OCF1A); 					// Clear timer1A compare flag
						TIMSK1|= (1<<OCIE1A); 				   	// Enable timer1A compare
						TIMSK1&=~(1<<OCIE1B); 				   	// Disable timer1B compare
								
						// Reset the OVF counter
						OVFt1OLD  = OVFt1;
						OVFt1 = 0;					
					}

					else	// RPM > 101 (10,100 RPM), false trigger
					{
						TIMSK1&=~(1<<OCIE1A); 				   	// Disable timer1B compare
						TIMSK1&=~(1<<OCIE1B); 				   	// Disable timer1B compare
					}
				}
				else	// OVF  = 0, false trigger
				{
					TIMSK1&=~(1<<OCIE1A); 				   	// Disable timer1B compare
					TIMSK1&=~(1<<OCIE1B); 				   	// Disable timer1B compare
				}
	
			}	// End !first_cycle
			
		// Clear the first cycle flag
			else  
			{
				first_cycle = 0;
				OVFt1 = 0;
				PORTD &= ~(_BV(IGN_TRIGGER));	//  LOW 
				TIMSK1&=~(1<<OCIE1A); 			// Disable timer1B compare
				TIMSK1&=~(1<<OCIE1B); 			// Disable timer1B compare
			}
			reluctor_trigger = 0;
		}// END reluctor_trigger
				
//********************************************************************
			if(t0OVF_trigger)	// 4Hz, 4 per second
			{
				t0OVF_trigger = 0;
				PORTD ^= (_BV(MAIN_LED));	//  TOGGLE
				EGT_check_flag = 1;
			}	
		sei();
					
			
		if(EGT_check_flag)
		{
			PORTB &= ~(_BV(MAXCHT));		// LOW to read data
			cht_h = SPI_ReceiveByte();
			cht_l = SPI_ReceiveByte();
			PORTB |= _BV(MAXCHT);		    //  HIGH starts new conversion
				
			PORTC &= ~(_BV(MAXEGT));		// LOW to read data
			egt_h = SPI_ReceiveByte();
			egt_l = SPI_ReceiveByte();
			PORTC |= _BV(MAXEGT);		    //  HIGH starts new conversion
			EGT_check_flag = 0;
		}
		//*********************************************************************************************
		if(CR_flag)
		{
			if( (!(RingBuffer_IsEmpty(&BTtoFF_Buffer))))
			{
				parse_SER_buffer(egt_h,egt_l,cht_h,cht_l,total_time);
			}
		}
		
	}
}

//*******************************************************************************
void KillStuff(){
			cli();	// disable interrupts
			// Turn everything off and clear pending interrupts
			TIMSK1&=~(1<<OCIE1A); 				   	// Disable timer1A compare
			TIMSK1&=~(1<<OCIE1B); 				   	// Disable timer1B compare
			TIMSK1&=~(1<<TOIE1); 				   	// Disable timer1 OVF
			TIMSK0&=~(1<<TOIE0); 					// Disable timer0 OVF
			UCSR1B&=~(1<<RXCIE1);					// Disable RX ISR
			EIMSK = 0x00;							// Disable all external interrupts
			TIFR1 = (1<<OCF1A); 					// Clear timer1A compare flag
			TIFR1 = (1<<OCF1B); 					// Clear timer1B compare flag
			TIFR1 = (1<<TOV1);						// Clear timer1 OVF flag
			TIFR0 = (1<<TOV0);						// Clear timer0 OVF flag
			EIFR = 0xFF;							// Clear all pending external interrupts
			UCSR1B = 0;
			UCSR1A = 0;
			UCSR1C = 0;
			UBRR1  = 0;
			CR_flag = 0;
			
			//LEDs_TurnOffLEDs(LEDS_LED1);
			PORTD &= ~(_BV(MAIN_LED));	//  LOW 
		}
//********************************************************************

/** Configures the board hardware and chip peripherals for the demo's functionality. */
void SetupHardware(void)
{
#if (ARCH == ARCH_AVR8)
	/* Disable watchdog if enabled by bootloader/fuses */
	MCUSR &= ~(1 << WDRF);
	wdt_disable();

	/* Disable clock division */
	clock_prescale_set(clock_div_1);
#elif (ARCH == ARCH_XMEGA)
	/* Start the PLL to multiply the 2MHz RC oscillator to 32MHz and switch the CPU core to run from it */
	XMEGACLK_StartPLL(CLOCK_SRC_INT_RC2MHZ, 2000000, F_CPU);
	XMEGACLK_SetCPUClockSource(CLOCK_SRC_PLL);

	/* Start the 32MHz internal RC oscillator and start the DFLL to increase it to 48MHz using the USB SOF as a reference */
	XMEGACLK_StartInternalOscillator(CLOCK_SRC_INT_RC32MHZ);
	XMEGACLK_StartDFLL(CLOCK_SRC_INT_RC32MHZ, DFLL_REF_INT_USBSOF, F_USB);

	PMIC.CTRL = PMIC_LOLVLEN_bm | PMIC_MEDLVLEN_bm | PMIC_HILVLEN_bm;
#endif

	/* Hardware Initialization */
	EXT_INT_init();
	TIMER0_init();
	TIMER1_init();
		
	// Initialize the serial USART driver before first use, with 9600 baud (and no double-speed mode)
	Serial_Init(9600, false);
	UCSR1B |= (1 << RXCIE1);	// Added this to enable RX ISR

	SPI_Init((SPI_SPEED_FCPU_DIV_8 | SPI_SCK_LEAD_RISING | SPI_SAMPLE_TRAILING | SPI_ORDER_MSB_FIRST | SPI_MODE_MASTER ) );
	USB_Init();
}

/** Event handler for the library USB Connection event. */
void EVENT_USB_Device_Connect(void)
{
	
}

/** Event handler for the library USB Disconnection event. */
void EVENT_USB_Device_Disconnect(void)
{
	
}

/** Event handler for the library USB Configuration Changed event. */
void EVENT_USB_Device_ConfigurationChanged(void)
{
	bool ConfigSuccess = true;

	ConfigSuccess &= HID_Device_ConfigureEndpoints(&Generic_HID_Interface);

	USB_Device_EnableSOFEvents();

	//LEDs_SetAllLEDs(ConfigSuccess ? LEDMASK_USB_READY : LEDMASK_USB_ERROR);
}

/** Event handler for the library USB Control Request reception event. */
void EVENT_USB_Device_ControlRequest(void)
{
	HID_Device_ProcessControlRequest(&Generic_HID_Interface);
}

/** Event handler for the USB device Start Of Frame event. */
void EVENT_USB_Device_StartOfFrame(void)
{
	HID_Device_MillisecondElapsed(&Generic_HID_Interface);
}

/** HID class driver callback function for the creation of HID reports to the host.
 *
 *  \param[in]     HIDInterfaceInfo  Pointer to the HID class interface configuration structure being referenced
 *  \param[in,out] ReportID    Report ID requested by the host if non-zero, otherwise callback should set to the generated report ID
 *  \param[in]     ReportType  Type of the report to create, either HID_REPORT_ITEM_In or HID_REPORT_ITEM_Feature
 *  \param[out]    ReportData  Pointer to a buffer where the created report should be stored
 *  \param[out]    ReportSize  Number of bytes written in the report (or zero if no report is to be sent)
 *
 *  \return Boolean \c true to force the sending of the report, \c false to let the library determine if it needs to be sent
 */
bool CALLBACK_HID_Device_CreateHIDReport(USB_ClassInfo_HID_Device_t* const HIDInterfaceInfo,
                                         uint8_t* const ReportID,
                                         const uint8_t ReportType,
                                         void* ReportData,
                                         uint16_t* const ReportSize)
{
	uint8_t* hidSendBuffer = (uint8_t*)ReportData;
	uint16_t tempMAP;
	uint8_t MAPindex;
	uint8_t MAPindex_start;
	uint8_t MAPindex_end;
	uint8_t BUFFERindex;
	static uint8_t msg_count = 0;
	
	switch(request_num)
	{
		case 0x8A:	// MAP got sent from host, handled in RX callback
			//cli();
			//KillStuff();
			hidSendBuffer[0]  = 0xAB;	// Header
			hidSendBuffer[1]  = 0xCD;
			hidSendBuffer[2]  = 0x8A;
			hidSendBuffer[3]  = (seq_num | 0x80);	// Echo back as an ACK
			hidSendBuffer[22] = 0xDE;	// Footer
			hidSendBuffer[23] = 0xAD;
			request_num = 0x00;
			seq_num = 0x00;
			*ReportSize = GENERIC_REPORT_SIZE;
			return true;
			break;
			
		case 0x8B:	//MAP being requested by host
			//cli();
			//KillStuff();
			hidSendBuffer[0]  = 0xAB;	// Header
			hidSendBuffer[1]  = 0xCD;
			hidSendBuffer[2]  = 0x8B;
			hidSendBuffer[3]  = (seq_num | 0x80);	// Echo back as an ACK
			//hidSendBuffer[22] = 0xDE;	// Footer
			//hidSendBuffer[23] = 0xAD;
		
			if(seq_num)
			{
				BUFFERindex = 4;
				MAPindex_start = (uint8_t)((seq_num-1)*10);
				MAPindex_end = MAPindex_start + 10;
				// put data into MSG from EEPROM
				if(MAPindex_end<101)	// Generic bounds checking, should add error code in msg
				{
					eeprom_busy_wait();
					if((PINB & 0x20))	// LOAD MAP 2
					{
						hidSendBuffer[29] = eeprom_read_byte((uint8_t*)&MAP2_STATIC);
					}
					else				// LOAD MAP 1
					{
						hidSendBuffer[29] = eeprom_read_byte((uint8_t*)&MAP1_STATIC);
					}
				
					for(MAPindex = MAPindex_start; MAPindex<MAPindex_end; MAPindex++)
					{
						eeprom_busy_wait();
						if((PINB & 0x20))	// LOAD MAP 2
						{
							tempMAP = eeprom_read_word((uint16_t*)&MAP2[MAPindex]);
						}
						else				// LOAD MAP 1
						{
							tempMAP = eeprom_read_word((uint16_t*)&MAP1[MAPindex]);
						}
						hidSendBuffer[BUFFERindex] = (tempMAP & 0xFF00) >> 8; // MSB
						++BUFFERindex;
						hidSendBuffer[BUFFERindex] = (tempMAP & 0x00FF);		// LSB
						++BUFFERindex;
					}
				}
			}
			request_num = 0x00;
			seq_num = 0x00;
			*ReportSize = GENERIC_REPORT_SIZE;	
			return true;
			//sei();
		break;
		
		case 0x01:
						hidSendBuffer[0]  = 0xAB;	// Header
						hidSendBuffer[1]  = 0xCD;
						hidSendBuffer[2]  = 0x01;	// MSG Type: Data Packet
						//  RPM and timing data
						hidSendBuffer[3]  = timer1H;
						hidSendBuffer[4]  = timer1L;
						hidSendBuffer[5]  = timer1_OVF_RPM;
						hidSendBuffer[6]  = tcA_high;
						hidSendBuffer[7]  = tcA_low;
						hidSendBuffer[8]  = tcB_high;
						hidSendBuffer[9]  = tcB_low;
						//	EGT and CHT data
						hidSendBuffer[16] = egt_h;
						hidSendBuffer[17] = egt_l;
						hidSendBuffer[18] = cht_h;
						hidSendBuffer[19] = cht_l;
						
						//	Current MAP number
						hidSendBuffer[20] = map_num;
						hidSendBuffer[21] = temp_rset;
						
						hidSendBuffer[22] = MAP_STATIC_TEMP;
						hidSendBuffer[23] = msg_count++;
						*ReportSize = GENERIC_REPORT_SIZE;	
		break;
		
		default:
			*ReportSize = 0;
		break;
			
	}

	
	return false;
}

/** HID class driver callback function for the processing of HID reports from the host.
 *
 *  \param[in] HIDInterfaceInfo  Pointer to the HID class interface configuration structure being referenced
 *  \param[in] ReportID    Report ID of the received report from the host
 *  \param[in] ReportType  The type of report that the host has sent, either HID_REPORT_ITEM_Out or HID_REPORT_ITEM_Feature
 *  \param[in] ReportData  Pointer to a buffer where the received report has been stored
 *  \param[in] ReportSize  Size in bytes of the received HID report
 */
void CALLBACK_HID_Device_ProcessHIDReport(USB_ClassInfo_HID_Device_t* const HIDInterfaceInfo,
                                          const uint8_t ReportID,
                                          const uint8_t ReportType,
                                          const void* ReportData,
                                          const uint16_t ReportSize)
{
	
	uint8_t* hidReceiveBuffer       = (uint8_t*)ReportData;
	uint16_t tempMAP;
	uint8_t MAPindex;
	uint8_t MAPindex_start;
	uint8_t MAPindex_end;
	uint8_t BUFFERindex;
	
	// Do we have a command request from the host?
	if (USB_ControlRequest.bRequest == HID_REQ_SetReport)
	{
	// Ensure this is the type of report we are interested in
	if (USB_ControlRequest.bmRequestType == (REQDIR_HOSTTODEVICE | REQTYPE_CLASS | REQREC_INTERFACE))
	{
	if((hidReceiveBuffer[0]==0xAB) &&(hidReceiveBuffer[1]==0xCD))
	{
		// Process GenericHID command packet
		switch(hidReceiveBuffer[2])	// THIS LOCATION NEED TO CHANGE
		{
					
			case 0x8A:	// Command 0x8A - Getting a MAP from HOST
				request_num = 0x8A;
				seq_num = hidReceiveBuffer[3];
				if(seq_num)
				{
					// pull data from MSG and put it into EEPROM
					BUFFERindex = 4;
					MAPindex_start = (uint8_t)((seq_num-1)*10);
					MAPindex_end = MAPindex_start + 10;
					// put data into MSG from EEPROM
					if(MAPindex_end<101)	// Generic bounds checking, should add error code in msg
					{
						eeprom_busy_wait();
						if((PINB & 0x20))	// LOAD MAP 2
						{
							eeprom_write_byte(((uint8_t*)&MAP2_STATIC),hidReceiveBuffer[29]);
						}
						else				// LOAD MAP 1
						{
							eeprom_write_byte(((uint8_t*)&MAP1_STATIC),hidReceiveBuffer[29]);
						}
						for(MAPindex = MAPindex_start; MAPindex<MAPindex_end; MAPindex++)
						{
							tempMAP = 0x00;
							tempMAP = (hidReceiveBuffer[BUFFERindex] << 8) + hidReceiveBuffer[BUFFERindex+1];
							eeprom_busy_wait();
							if((PINB & 0x20))	// LOAD MAP 2
							{
								eeprom_write_word(((uint16_t*)&MAP2[MAPindex]), tempMAP);
							}
							else				// LOAD MAP 1
							{
								eeprom_write_word(((uint16_t*)&MAP1[MAPindex]), tempMAP);
							}
							
							++BUFFERindex;
							++BUFFERindex;
						}
					}
				}
				
			break;
					
			case 0x8B:	// Command 0x8B - Send MAP to HOST
				request_num = 0x8B;
				seq_num = hidReceiveBuffer[3];
			break;
					
			case 0x8C:	// Command 0x8C - Set MODE to FW loading, i.e. jump to bootloader
				cli();
				Jump_To_Bootloader();
					
			break;
					
			case 0x8F:	// Command 0x8F - Reset FF
				cli();
				first_cycle = 1;	// restarts data collection cycle
				request_num = 0x01;	// Gets us sending back data packets
				sei();
			break;
					
			default:
			// Unknown command received
			break;
		} // End of switch(hidReceiveBuffer[0])
	}
	}
	}
}



//***********************************************************
//***********************************************************
//*****													*****
//*****		START OF THE ISR's							*****
//*****													*****
//***********************************************************
//***********************************************************

// This interrupt routine is run approx 61 times per second.
ISR(TIMER0_OVF_vect)
{
	static uint8_t count=0;
	// Interrupt after 156 counts for 100 Hz, so preload 256-156 = 100
	// ((16000000/1024)/ 100) = 156
	TCNT0 = 100;

	if(++count >25)	// 100hz/ 25 = 250 msec
	{
		t0OVF_trigger = 1;
		ISR_flags = 1;
		count = 0;
	}

}

ISR(TIMER1_COMPA_vect)
{

	// THIS IS WHERE THE TIMER1 OUTPUT MATCHES THE PRELOADED OUTPUT
	// FROM PREVIOUS RPM CALC AND ADV RET CALC
	// SO ITS TIME TO TRIGGER THE CDI
	
	if(!first_cycle)
	{
		// 	Get the high and low count from timer 1
		// 	Grab them a byte at a time to make it easier to send
		tcA_low  = TCNT1L;
		tcA_high = TCNT1H;
		PORTD |= _BV(IGN_TRIGGER);	//  HIGH 
		// Disable CompA, Enable CompB
		TIMSK1&=~(1<<OCIE1A); 				   	// Disable timer1A compare
		TIFR1 = (1<<OCF1B); 					// Clear timer1B compare flag
		TIMSK1 |= (1<<OCIE1B); 					// Enable timer1B compare
	}
	
	else 
	{
		// Disable compA&B
		TIMSK1&=~(1<<OCIE1A); 				   	// Disable timer1A compare
		TIMSK1&=~(1<<OCIE1B); 				   	// Disable timer1B compare
	}	
}


ISR(TIMER1_COMPB_vect)
{
	PORTD &= ~(_BV(IGN_TRIGGER));		//  LOW fires at this point?
	TIMSK1&=~(1<<OCIE1A); 				// Disable timer1A compare
	TIMSK1&=~(1<<OCIE1B); 				// Disable timer1B compare
	
	if(!first_cycle)
	{
		tcB_low  = TCNT1L;
		tcB_high = TCNT1H;
		send_flag = 1;					//  SEND THE DATA
		ISR_flags = 1;
	}
}


ISR(TIMER1_OVF_vect)
{
	//t1OVF_trigger = 1;
	//ISR_flags = 1;
	++timer1_OVF_RPM;
}

//*************************************************************
//	MAX9924 triggered input from reluctor
//*************************************************************
ISR(INT1_vect)  
{
	if(!reluctor_trigger)	// If your not already set (sort of a debounce?)
	{
	// 	Get the high and low count from timer 1
	// 	Grab them a byte at a time to make it easier to send
	// 	via serial port. Might be a smarter way to do this later.
		timer1L  = TCNT1L;
		timer1H  = TCNT1H;
		TCNT1 = 0;				// 	RESET timer1
		reluctor_trigger = 1;	// signal main to handle CDI trigger
		ISR_flags = 1;
	}
}

//*************************************************************
//	ISR to manage the reception of data from the serial port 
//	placing received bytes into a circular buffer
//*************************************************************
ISR(USART1_RX_vect)
{	
	uint8_t ReceivedByte = UDR1;	
	if(ReceivedByte == 0x20)	{return;}		// Ignore spaces
	if(ReceivedByte == 0x0D)	{CR_flag = 1;}	// Received a Carriage Return
	RingBuffer_Insert(&BTtoFF_Buffer, ReceivedByte);
	
}


ISR(BADISR_vect)
{
	bad_count++;
}


