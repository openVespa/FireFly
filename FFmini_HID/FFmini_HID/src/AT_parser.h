#ifndef __AT_PARSER_H__
#define __AT_PARSER_H__

#include <avr/io.h>
#include <avr/interrupt.h>
#include <avr/pgmspace.h>
#include <LUFA/Drivers/Peripheral/Serial.h>
#include <LUFA/Drivers/Misc/RingBuffer.h>

volatile uint8_t CR_flag;
/** Circular buffer to hold data from FFmini to BlueTooth SPP device*/
RingBuffer_t FFtoBT_Buffer;
/** Underlying data buffer for FFmini, where the bytes to be TD'd are located. */
uint8_t      FFtoBT_Buffer_Data[32];
/** Circular buffer to hold data from BlueTooth SPP device to FFmini*/
RingBuffer_t BTtoFF_Buffer;
/** Underlying data buffer for BlueTooth SPP device, where RX'd bytes are located. */
uint8_t      BTtoFF_Buffer_Data[32];


void send_ELM327_prompt(void);
void send_ELM327_OK(void);
void send_ELM327_CR(void);
void parse_SER_buffer(uint8_t EGT_H, uint8_t EGT_L, uint8_t CHT_H, uint8_t CHT_L, uint32_t total_time_RPM);
void send_ELM327_header(void);


#endif
