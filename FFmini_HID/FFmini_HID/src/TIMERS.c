//	Timer(s) code
#include "TIMERS.h"
#include <avr/io.h>
#include <avr/interrupt.h>


void TIMER0_init()
{	
        // Configure timer 0 to generate a timer overflow interrupt every
        // 256*1024 clock cycles, or approx 61 Hz when using 16 MHz clock
		// Seed clock with (256-156) = 100 for 100 Hz clock
        TCCR0A = 0x00;
        TCCR0B = 0x05;
		TCNT0 = 100;
        TIMSK0 = (1<<TOIE0);
}

void TIMER1_init()
{
	//	Initialize the 16-bit Timer1 to clock at 16 MHz
	TCCR1A = 0x00;							// Prescale Timer1 @ 1
	TCCR1B = 0x01;	
	TIFR1&=~(1<<TOV1);						// Clear overflow flag
	TIMSK1 |= (1<<TOIE1); 				   	// start timer
	TIMSK1&=~(1<<OCIE1A); 				   	// Disable timer1A compare
	TIMSK1&=~(1<<OCIE1B); 				   	// Disable timer1B compare
}


