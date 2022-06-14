CREATE TABLE [dbo].[City]
(
    [Id] INT NOT NULL PRIMARY KEY,
	[Description] [nvarchar](200) NOT NULL,
	[Deep] [int] NOT NULL Default(0),
)
CREATE TABLE [dbo].[StatusDescription]
(
	[Id] INT NOT NULL PRIMARY KEY,
	[Description] [nvarchar](200) NOT NULL,
)
CREATE TABLE [dbo].[Config]
(
	[Id] INT NOT NULL PRIMARY KEY,
	[DockerId] [nvarchar](500) NOT NULL,
	[Port] [nvarchar](6) NOT NULL,
    [MongooseUrl] [nvarchar](600) NOT NULL,
	[TickMinutes] [int] NOT NULL Default(0),
)
CREATE TABLE [dbo].[Status]
(
	[Id] INT NOT NULL PRIMARY KEY,
	[IdConfig] INT NOT NULL ,
	[IdStatusDescription] INT NOT NULL,
    [DeviceIp] [nvarchar](200) NOT NULL,
	[Message] [nvarchar](Max)  NULL,
	[City] [nvarchar](200) NULL,
	[LastSyncDate] [DateTime] NOT NULL,
	CONSTRAINT [FK_Status_ToConfig] FOREIGN KEY ([IdConfig]) REFERENCES [dbo].[Config] ([Id]),
	CONSTRAINT [FK_Status_ToStatusDescription] FOREIGN KEY ([IdStatusDescription]) REFERENCES [dbo].[StatusDescription] ([Id]),
)

INSERT [dbo].[StatusDescription] ([Id],[Description]) VALUES (1,'Alive')
INSERT [dbo].[StatusDescription] ([Id],[Description]) VALUES (2,'Queued')
INSERT [dbo].[StatusDescription] ([Id],[Description]) VALUES (3,'Busy')
INSERT [dbo].[StatusDescription] ([Id],[Description]) VALUES (4,'Error')
CREATE TABLE [dbo].[Log]
(
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Data]  [DateTime] NOT NULL,
	[DockerID] [nvarchar](100) NOT NULL,
	[Message]  [nvarchar](500) NOT NULL,
	[LogType]  [nvarchar](20) NOT NULL,
	[LastSyncDate] [DateTime]  NULL,
    CONSTRAINT [PK_Log] PRIMARY KEY CLUSTERED ([Id] ASC),
)