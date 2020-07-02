# Euston Leisure Messaging

## Brief

The system developed must be simple to use and intuitive. It must allow for manual input of messages and for reading messages from a text file.

The system must display four lists: a trending list based on the hashtags contained in Twitter messages, a mentions list based on the Twitter user IDs contained in Twitter messages, a Significant Incident Report list based on the emails received, and a URL Quarantine list.

- Text messages: the system must identify the phone number of the sender and textspeak abbreviations must be expanded to their full form.
- Email messages: the system must identify the email address of the sender and URLs contained in the message body must be removed from the message. There can be two types of email messages: standard emails and Significant Incident Reports (SIR). SIRs must be listed and appear on the GUI.
- Twitter messages: the system must identify the twitter ID of the sender. Textspeak abbreviations must be expanded, hashtags must be counted and stored in a list. IDs must be stored in a list. Both lists must appear on the GUI.

All messages must be serialised in the JSON format and written in a JSON file.

## Solution developed and output

![](https://raw.githubusercontent.com/musevarg/Euston-Leisure/master/pic1.png)
![](https://raw.githubusercontent.com/musevarg/Euston-Leisure/master/pic2.png)
