const int SW_Pin = 2;
const int ledPINGREEN = 4;
const int ledPINRED = 3;
const int x_pot = A0;
const int y_pot = A1;

int buttonState = HIGH;
int lastButtonState = HIGH;

void setup() {
    Serial.begin(9600);

    pinMode(x_pot, INPUT);
    pinMode(y_pot, INPUT);

    pinMode(ledPINGREEN, OUTPUT);
    pinMode(ledPINRED, OUTPUT);
    pinMode(SW_Pin, INPUT_PULLUP);

    digitalWrite(ledPINGREEN, LOW);
    digitalWrite(ledPINRED, LOW);

    
}

void loop() {
    int current_y= analogRead(y_pot);
    int current_x= analogRead(x_pot);
    buttonState = digitalRead(SW_Pin);

    if (buttonState != lastButtonState) {
        if (buttonState == LOW) {
            Serial.println("ACTIVE");
        } 
        else {
            Serial.println("NONACTIVE");
        }

        lastButtonState = buttonState;
    }
    if (current_y > 1000){
        Serial.println("DOWN");
        current_y= analogRead(y_pot);
    }

    else if (current_x > 1000){
        Serial.println("RIGHT");
        current_x= analogRead(x_pot);
    }

    else if (current_y < 50){
        Serial.println("UP");
        current_y= analogRead(y_pot);
    }

    else if (current_x < 50){
        Serial.println("LEFT");
        current_x= analogRead(x_pot);
    }

    if (Serial.available() > 0) {
        char command = Serial.read();

        if (command == '1') {
            digitalWrite(ledPINGREEN, HIGH);
            delay(1000);
            digitalWrite(ledPINGREEN, LOW);
        }
        else if (command == '0') {
            digitalWrite(ledPINGREEN, LOW);
            digitalWrite(ledPINRED, LOW);
        }
        else if (command == '2') {
            digitalWrite(ledPINRED, HIGH);
            delay(1000);
            digitalWrite(ledPINRED, LOW);
        }
    }
    delay(10);
}
