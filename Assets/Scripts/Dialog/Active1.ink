INCLUDE Globals.ink

-> active

== active ==
# attribute: active
Have you seen the mice around the cafe?
It’s so fun chasing them around and catching them, it’s like we’re playing tag!
    + "Mice are bad for the cafe, you should kill them."
        -> active_notnice
    + "That sounds like fun, keep it up!"
        -> active_nice

== active_notnice ==
# affinity: -1
What!?
But if the mice are gone then how will I chase mice?
It’s kind of my job since I’m, y’kno…a cat.
    -> END

== active_nice ==
# affinity: 1
It really is!
But sometimes the mice get too tired to keep playing :(
No one can keep up with me!
    -> END 