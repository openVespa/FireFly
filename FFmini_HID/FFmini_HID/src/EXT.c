// External interrupt code
#include "EXT.h"
#include <avr/io.h>
#include <avr/interrupt.h>


void EXT_INT_init(void){

	//	 interrupt on INT1 pin rising edge (MAX9924 triggered) 
	//EICRA = (1<<ISC31) | (1<<ISC30) | (1<<ISC21) | (1<<ISC20)| (1<<ISC11) | (1<<ISC10) | (1<<ISC01) | (1<<ISC00);
	
	//	 interrupt on INT1 pin falling edge (MAX9924 triggered) 
	EICRA = (1<<ISC31) | (0<<ISC30) | (1<<ISC21) | (0<<ISC20)| (1<<ISC11) | (0<<ISC10) | (1<<ISC01) | (0<<ISC00);
	
	EIMSK =  (1<<INT1);	// Enable MAX9924 
	
	
	}
	
