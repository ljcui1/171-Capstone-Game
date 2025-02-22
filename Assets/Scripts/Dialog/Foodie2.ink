INCLUDE Globals.ink

-> foodie

== foodie ==
# attribute: foodie
I’m so full from dinner, the wet food today was so yummy :3
    + "Now that you’ve had a good meal, it’s time to tuck in for the night!"
        -> foodie_notnice
    + "It’s a good idea to take a stroll around the cafe to digest."
        -> foodie_nice

== foodie_notnice ==
# affinity: -1
Noo! but I want to play some more!
    -> END

== foodie_nice ==
# affinity: 1
You’re right, once my food is digested I can play some more!! 
    -> END
