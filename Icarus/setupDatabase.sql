CREATE TABLE `discordguildconfig` (
  `DiscordGuildConfigId` int NOT NULL AUTO_INCREMENT,
  `GuildId` bigint DEFAULT NULL,
  `Key` varchar(45) DEFAULT NULL,
  `Value` varchar(4096) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `SetByMemberId` int DEFAULT NULL,
  `SetAt` datetime DEFAULT NULL,
  PRIMARY KEY (`DiscordGuildConfigId`),
  UNIQUE KEY `guildconfigid_UNIQUE` (`DiscordGuildConfigId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE `discorduserupdate` (
  `DiscordUserUpdateId` int NOT NULL AUTO_INCREMENT,
  `MemberId` int DEFAULT NULL,
  `UpdatedAttribute` varchar(45) DEFAULT NULL,
  `Data` varchar(8192) DEFAULT NULL,
  `FileId` int DEFAULT NULL,
  PRIMARY KEY (`DiscordUserUpdateId`),
  UNIQUE KEY `UserUpdateId_UNIQUE` (`DiscordUserUpdateId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE `file` (
  `FileId` int NOT NULL AUTO_INCREMENT,
  `FileName` varchar(255) DEFAULT NULL,
  `FileSizeBytes` bigint DEFAULT NULL,
  `CreatedAt` datetime DEFAULT NULL,
  PRIMARY KEY (`FileId`),
  UNIQUE KEY `FileId_UNIQUE` (`FileId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE `filter` (
  `FilterId` int NOT NULL AUTO_INCREMENT,
  `Type` int DEFAULT NULL,
  `FilterText` varchar(4096) DEFAULT NULL,
  `FilterTextConverted` varchar(4096) DEFAULT NULL,
  `AddedByMemberId` int DEFAULT NULL,
  `AddTime` datetime DEFAULT NULL,
  PRIMARY KEY (`FilterId`),
  UNIQUE KEY `FilterId_UNIQUE` (`FilterId`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE `filterexception` (
  `FilterExceptionId` int NOT NULL AUTO_INCREMENT,
  `FilterId` int DEFAULT NULL,
  `ExceptionText` varchar(4096) DEFAULT NULL,
  `AddedByMemberId` int DEFAULT NULL,
  `AddTime` datetime DEFAULT NULL,
  PRIMARY KEY (`FilterExceptionId`),
  KEY `filterexception_filter_idx` (`FilterId`),
  CONSTRAINT `filterexception_filter` FOREIGN KEY (`FilterId`) REFERENCES `filter` (`FilterId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE `flag` (
  `FlagId` int NOT NULL AUTO_INCREMENT,
  `Time` datetime DEFAULT NULL,
  `Type` int DEFAULT NULL,
  `FlaggedByMemberId` int DEFAULT NULL,
  `SourceMessageId` bigint DEFAULT NULL,
  `SourceChannelId` bigint DEFAULT NULL,
  `SourceUserId` bigint DEFAULT NULL,
  `SourceContent` text,
  `SourceMatches` varchar(4096) DEFAULT NULL,
  `ResolvedByMemberId` int DEFAULT NULL,
  `ResolutionTime` datetime DEFAULT NULL,
  `ResolutionType` int DEFAULT NULL,
  `ResolutionPoints` int DEFAULT NULL,
  `SystemMessage` varchar(512) DEFAULT NULL,
  PRIMARY KEY (`FlagId`),
  UNIQUE KEY `FlagId_UNIQUE` (`FlagId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE `member` (
  `MemberId` int NOT NULL AUTO_INCREMENT,
  `DiscordId` bigint DEFAULT NULL,
  `GuildedId` varchar(8) DEFAULT NULL,
  `CreatedAt` datetime DEFAULT NULL,
  PRIMARY KEY (`MemberId`),
  UNIQUE KEY `MemberId_UNIQUE` (`MemberId`),
  UNIQUE KEY `DiscordId_UNIQUE` (`DiscordId`),
  UNIQUE KEY `GuildedId_UNIQUE` (`GuildedId`)
) ENGINE=InnoDB AUTO_INCREMENT=13 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE `tag` (
  `TagId` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(45) DEFAULT NULL,
  `Content` varchar(4096) DEFAULT NULL,
  `AttachmentURL` varchar(2000) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `IsEmbed` bit(1) DEFAULT NULL,
  `CreatedByMemberId` int DEFAULT NULL,
  `CreatedAt` datetime DEFAULT NULL,
  PRIMARY KEY (`TagId`),
  UNIQUE KEY `TagId_UNIQUE` (`TagId`),
  UNIQUE KEY `TagName_UNIQUE` (`Name`)
) ENGINE=InnoDB AUTO_INCREMENT=39 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

