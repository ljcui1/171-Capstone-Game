INCLUDE Globals.ink

-> foodie

== foodie ==
# attribute: foodie
Hellooo!
Has the human in the apron refilled our food bowls yet?!?
I’m craving kibble like crazy!!!
    + "I can go check for you, if you want?"
        -> foodie_nice
    + "Dinner was just served, how do you already want more?"
        -> foodie_notnice

= foodie_nice 
# affinity: 1
Yes please, I love eating so much!
The flavors of the kibble mixed with bone broth complement each other perfectly, I’m so lucky to get to eat such culinary delights every day here!
I hope I find a human that loves food just as much as I do :3
    -> END

= foodie_notnice
# affinity: -1
THAT WAS DINNER??
It felt like just a snack to me!
My palette is already cleansed and ready for the next wonderful meal the human in the apron will serve us… 
I guess I could just chat with you for the time being!
    -> END
