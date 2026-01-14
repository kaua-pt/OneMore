USE onemore;
CREATE TABLE IF NOT EXISTS `__EFMigrationsHistory` (
    `MigrationId` varchar(150) CHARACTER SET utf8mb4 NOT NULL,
    `ProductVersion` varchar(32) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK___EFMigrationsHistory` PRIMARY KEY (`MigrationId`)
) CHARACTER SET=utf8mb4;

START TRANSACTION;
ALTER DATABASE CHARACTER SET utf8mb4;

CREATE TABLE `Words` (
    `Id` char(36) COLLATE ascii_general_ci NOT NULL,
    `Category` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `Text` varchar(200) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK_Words` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20260102092847_02012026_06h28m39s', '9.0.9');

COMMIT;

