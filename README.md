# .NET MAUI app using Stream Chat
Example of using Stream Chat in a .NET MAUI application.

### This demo project currently presents how to:
1. Connect to the Stream Chat Server
2. Query channels
3. Show channel messages
4. Send new messages

## How to run this project?
1. Create Stream account and start trial
2. Go to [Stream's Dashboard](https://dashboard.getstream.io/) and:
    1. Create new app
    2. [Enable Developer Tokens](https://getstream.io/chat/docs/unity/tokens_and_authentication/?language=unity#developer-tokens) (Authorization will not work in this example otherwise!)
    3. Take the `Api Key` of your App and save in [StreamApiKey field of the Static config](https://github.com/sierpinskid/stream-chat-dotnet-maui/blob/main/StreamChatMaui/StaticConfig.cs) (please note that `api key` is different from `api secret`)
3. Run the application

## Stream Chat Service
The key component that initiates a connection with the Stream Chat Server is the [Stream Chat Service](https://github.com/sierpinskid/stream-chat-dotnet-maui/blob/main/StreamChatMaui/Services/StreamChatService.cs). There are 3 actions required throughout a single session of the app running:
1. Initiate the connection with `streamClient.ConnectUserAsync(credentials);`
2. Update the client continously with `streamClient.Update();` (This updates the WebSocket communication)
3. Call `streamClient.Dispose()` when finished using to free up resources
All of the above are already covered by the provided [Stream Chat Service](https://github.com/sierpinskid/stream-chat-dotnet-maui/blob/main/StreamChatMaui/Services/StreamChatService.cs)..

### Tested on:
- Windows
- Android Emulator
- Android Device

### Steps taken to create this project
1. Create new .NET MAUI project
2. Add [Newtonsoft.Json](https://www.nuget.org/packages/Newtonsoft.Json/) package
3. Add Stream Chat SDK
4. Add [MAUI Community Toolkit](https://learn.microsoft.com/en-us/dotnet/communitytoolkit/maui/get-started?tabs=CommunityToolkitMaui)

### Conventions
Each class members are sorted:
- by accessability modifier: public -> protected -> private
- then by member type: properties -> fields -> methods
