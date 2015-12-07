/*
 * IRremote: IRrecvDemo - demonstrates receiving IR codes with IRrecv
 * An IR detector/demodulator must be connected to the input RECV_PIN.
 * Version 0.1 July, 2009
 * Copyright 2009 Ken Shirriff
 * http://arcfn.com
 */

#include <IRremote.h>
#include <string.h>

int RECV_PIN = 10;

IRrecv irrecv(RECV_PIN);

#define DELIMETER '~'

decode_results results;

void setup()
{
  Serial.begin(9600);
  irrecv.enableIRIn(); // Start the receiver
}

void loop() {
  if (irrecv.decode(&results)) {
    bool repeat = false;
    if (results.value == REPEAT)
      repeat = true;
    int bits = results.bits;
    int count = results.rawlen;

    Serial.print(DELIMETER);
    Serial.print("{");
    Serial.print("\"valuehi\":");
    char hexstring[20];
    long unsigned int valuehi = results.value;
    long unsigned int valuelo = valuehi & 0xFFFF;
//    if ((bits > 16) || repeat )
//    {
//      //Serial.print("\\u");
      valuehi = valuehi >> 16;
//      sprintf( hexstring, "%04X", value1 );
//      //Serial.print(hexstring);
    Serial.print(valuehi);
   // }
    //Serial.print("\\u");
    //sprintf( hexstring, "%04X", value2 );
    //Serial.print(hexstring);
    Serial.print(",\"valuelo\":");
    Serial.print(valuelo);
    Serial.print(",\"decode_type\":");
    Serial.print(results.decode_type);
    Serial.print(",\"address\":");
    Serial.print(results.address);
    Serial.print(",\"bits\":");
    Serial.print(bits);
    Serial.print(",\"rawlen\":");
    Serial.print(results.rawlen);
    Serial.print(",\"overflow\":");
    Serial.print(results.overflow);
    Serial.print(",\"repeat\":");
    Serial.print(repeat);
    Serial.print(",");
    if (!repeat)
    {
      //Serial.print("{");
      Serial.print("\"Raw\":");
      Serial.print("[");

      for (int i = 1; i < count; i++) {
        unsigned long val = results.rawbuf[i];
        val *= USECPERTICK;
        //char valbuf[20];
        //sprintf(valbuf, "\"\\u%04X\",", val);
        Serial.print(val);
        //No last comma
        if (i!= count-1)
          Serial.print(',');
      }
      Serial.print("]");
    }
    Serial.print("}");
    Serial.print(DELIMETER);
    irrecv.resume(); // Receive the next value
  }
  delay(100);
}
