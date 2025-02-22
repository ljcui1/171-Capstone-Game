INCLUDE Globals.ink

-> talkative

== talkative ==
# attribute: talkative
Hi!
Hello!
How was your day?
You really do a great job managing this cafe and I’m really happy getting to meet and talk to so many new humans every day. 
They seem to love listening to me talk but it seems like I can never find anyone that can keep up with the conversation!
    + "That’s surprising, I thought you just loved hearing your own voice."
        -> talkative_notnice
    
    + "That’s a shame, it’s always nice to find someone to have engaging talks with. I’ll try my best to keep my eye out for you!"
        -> talkative_nice


= talkative_notnice 
# affinity: -1
That wasn’t very nice to say. I wouldn’t like to have a conversation with someone like you.
    -> END


= talkative_nice
# affinity: 1
You’d do that?? 
Thanks so much! 
It would be my dream to spend my days engrossed in conversation with my human and appreciating each other’s input and company!!
    -> END

