# Serverless Ticketing Service


This project contains the source code for Codemotion EDU Path. You can find the path <a href="https://talks.codemotion.com/paths/realizzare-un-sistema-di-gestione-ticket-1" target="_blank">here</a> (it is in Italian language)

The Codemotion EDU-Path is composed by 10 episode, you can find the source code for each episode in one of the following branches.

1) <a href="https://github.com/massimobonanni/ServerlessTicketingService/tree/01-Start" target="_blank">Branch 01-Start</a> the initial project (<a href="https://talks.codemotion.com/architettura-della-soluzione-tools-e-cre?playlist=realizzare-un-sistema-di-gestione-ticket-1" target="_blank">episode 2</a>)

2) <a href="https://github.com/massimobonanni/ServerlessTicketingService/tree/02-API_Rest" target="_blank">Branch 02-API_Rest</a> implementation of the frontend APIs for tickets management (<a href="https://talks.codemotion.com/implementare-le-rest-api-di-gestione-dei?playlist=realizzare-un-sistema-di-gestione-ticket-1" target="_blank">episode 3</a>)

3) <a href="https://github.com/massimobonanni/ServerlessTicketingService/tree/03-OpenAPI" target="_blank">Branch 03-OpenApi</a> adding the REST API documentation using OpenAPI package for Azure Functions (<a href="https://talks.codemotion.com/documentare-le-api-di-gestione-dei-ticke?playlist=realizzare-un-sistema-di-gestione-ticket-1" target="_blank">episode 4</a>)

4) <a href="https://github.com/massimobonanni/ServerlessTicketingService/tree/04-TicketEntity" target="_blank">Branch 04-TicketEntity</a> implementation of durable entity to manage a single ticket and interaction between front-end APIs and Durable Entity (<a href="https://talks.codemotion.com/gestire-lo-stato-dei-ticket-tramite-dura?playlist=realizzare-un-sistema-di-gestione-ticket-1" target="_blank">episode 5</a> <a href="https://talks.codemotion.com/interagire-con-la-durable-entity?playlist=realizzare-un-sistema-di-gestione-ticket-1" target="_blank">episode 6</a>)

5) <a href="https://github.com/massimobonanni/ServerlessTicketingService/tree/05-Search_API" target="_blank">Branch 05-Search_API</a> adding search feature in the REST API using Durable Functions capabilities (<a href="https://talks.codemotion.com/implementare-le-funzionalit-di-ricerca-d?playlist=realizzare-un-sistema-di-gestione-ticket-1" target="_blank">episode 7</a>) 

6) <a href="https://github.com/massimobonanni/ServerlessTicketingService/tree/06-Update_Orchestration" target="_blank">Branch 06-Update_Orchestration</a> implementation of the orchestrator function to manage notifications for a ticket update (<a href="https://talks.codemotion.com/gestire-e-persistere-levento-di-aggiorna?playlist=realizzare-un-sistema-di-gestione-ticket-1" target="_blank">episode 8</a>)

7) <a href="https://github.com/massimobonanni/ServerlessTicketingService/tree/07-Update_Events" target="_blank">Branch 07_Update_Event</a> deploy of a custom topic in Event Grid and an activity to manage the notification of a ticket update to external services (<a href="https://talks.codemotion.com/emettere-gli-eventi-di-aggiornamento-su-?playlist=realizzare-un-sistema-di-gestione-ticket-1" target="_blank">episode 9</a>)

8) <a href="https://github.com/massimobonanni/ServerlessTicketingService/tree/08-Notification-LogicApp" target="_blank">Branch 08-Notification-LogicApp</a> creation of a Logic App to subscribe the custom topic created in the previous episode to send email (<a href="https://talks.codemotion.com/notificare-laggiornamento-di-un-ticket-v?playlist=realizzare-un-sistema-di-gestione-ticket-1" target="_blank">episode 10</a>)
