-- MySQL dump 10.13  Distrib 8.0.33, for Win64 (x86_64)
--
-- Host: 127.0.0.1    Database: art-critique-db
-- ------------------------------------------------------
-- Server version	8.0.33

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `t_painting_genre`
--

DROP TABLE IF EXISTS `t_painting_genre`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `t_painting_genre` (
  `genreID` int NOT NULL AUTO_INCREMENT,
  `genreName` varchar(50) NOT NULL,
  `genreDescription` varchar(200) DEFAULT NULL,
  PRIMARY KEY (`genreID`),
  UNIQUE KEY `idt_painting_genre_UNIQUE` (`genreID`)
) ENGINE=InnoDB AUTO_INCREMENT=23 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `t_painting_genre`
--

LOCK TABLES `t_painting_genre` WRITE;
/*!40000 ALTER TABLE `t_painting_genre` DISABLE KEYS */;
INSERT INTO `t_painting_genre` VALUES (1,'Other',NULL),(2,'Renaissance',NULL),(3,'Rococo',NULL),(4,'Romanticism',NULL),(5,'Impressionism',NULL),(6,'Expressionism',NULL),(7,'Surrealism',NULL),(8,'Abstract Art',NULL),(9,'Bauhaus Art',NULL),(10,'Pop Art',NULL),(11,'Realist Art',NULL),(12,'Art Deco',NULL),(13,'Classicism',NULL),(14,'Contemporary Art',NULL),(15,'Cubism',NULL),(16,'Gothic Art',NULL),(17,'Modernism',NULL),(18,'Minimalism',NULL),(19,'Neo-Baroque',NULL),(20,'Neo-romanticism',NULL),(21,'Neoclassicism',NULL),(22,'Photorealism',NULL);
/*!40000 ALTER TABLE `t_painting_genre` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2023-06-15 20:33:17
