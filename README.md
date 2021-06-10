
# CodeChallenges

## 1. The Post Lock-down Party! 
  
### Imagine… 
### It is summer 2021. 
95% of the population have been vaccinated. 
Lock-down will end soon and you’re planning a party! 
 
But not everyone gets along… So this year, you decide, will be different. You're going to find the optimal seating arrangement and avoid any awkwardness. 
 
You start by writing up a list of everyone invited and the amount their happiness would increase or decrease if they were to find themselves sitting next to each other person. You have a circular table that will be just big enough to fit everyone comfortably, and so each person will have exactly two neighbours. 
 
For example, suppose you have only four attendees planned, and you calculate their potential happiness as follows: 
 
```
Alice would gain 54 happiness units by sitting next to Bob. 
Alice would lose 79 happiness units by sitting next to Carol. 
Alice would lose 2 happiness units by sitting next to David. 
Bob would gain 83 happiness units by sitting next to Alice. 
Bob would lose 7 happiness units by sitting next to Carol. 
Bob would lose 63 happiness units by sitting next to David. 
Carol would lose 62 happiness units by sitting next to Alice. 
Carol would gain 60 happiness units by sitting next to Bob. 
Carol would gain 55 happiness units by sitting next to David. 
David would gain 46 happiness units by sitting next to Alice. 
David would lose 7 happiness units by sitting next to Bob. 
David would gain 41 happiness units by sitting next to Carol. 
```
 
Then, if you seat Alice next to David, Alice would lose 2 happiness units (because David talks so much), but David would gain 46 happiness units (because Alice is such a good listener), for a total change of 44. 
 
If you continue around the table, you could then seat Bob next to Alice (Bob gains 83, Alice gains 54). Finally, seat Carol, who sits next to Bob (Carol gains 60, Bob loses 7) and David (Carol gains 55, David gains 41). The arrangement looks like this: 
 
 ```
      +41    +46 
+55    David    -2 
Carol              Alice 
+60     Bob    +54 
         -7   +83 
```

After trying every other seating arrangement, you find that this one is the most optimal, with a total change in happiness of 330. 
 
**What is the total change in happiness for the optimal seating arrangement of the actual guest list?**
<br>
## 2. Password Shenanigans  

To help you remember your new password after the old one expires, you have devised a method of coming up with a password based on the previous one.  

A new NBS policy dictates that passwords must be exactly eight lowercase letters (for security reasons), so you find a new password by incrementing your old password string repeatedly until it is valid. 

Incrementing is just like counting with numbers: **xx, xy, xz, ya, yb**, and so on. Increase the rightmost letter one step; if it was z, it wraps around to a, and repeat with the next letter to the left until one doesn't wrap around. 

Unfortunately, the new Security Manager has imposed some additional password requirements! 

Passwords must include one increasing straight of at least three letters, like **abc, bcd, cde**, and so on, up to xyz. They cannot skip letters; abd doesn't count. 

Passwords may not contain the letters **i**, **o**, or **l**, as these letters can be mistaken for other characters and are therefore confusing. 

Passwords must contain at least two different, non-overlapping pairs of letters, like **aa, bb,** or **zz**. 

For example: 

* **hijklmmn** meets the first requirement (because it contains the straight hij) but fails the second requirement (because it contains **i** and **l**). 

* **abbceffg** meets the third requirement (because it repeats **bb** and **ff**) but fails the first requirement. 

* **abbcegjk** fails the third requirement, because it only has one double letter (**bb**). 

The next password after **abcdefgh** is **abcdffaa**. 

The next password after **ghijklmn** is **ghjaabcc** because you eventually skip all the passwords that start with **ghi**..., since **i** is not allowed.

## Challenge:
Given your current password **hxbxwxba**, what should his next password be? 
