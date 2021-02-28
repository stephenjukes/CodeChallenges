# Password Shenanigans  

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