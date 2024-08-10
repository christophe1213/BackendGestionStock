-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Hôte : 127.0.0.1:3306
-- Généré le : mer. 07 août 2024 à 06:34
-- Version du serveur : 8.2.0
-- Version de PHP : 8.2.13

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Base de données : `gestion_stock`
--

-- --------------------------------------------------------

--
-- Structure de la table `client`
--

DROP TABLE IF EXISTS `client`;
CREATE TABLE IF NOT EXISTS `client` (
  `idclient` varchar(20) NOT NULL,
  `NomClient` varchar(200) DEFAULT NULL,
  `Adresse` varchar(100) DEFAULT NULL,
  `Numtel` varchar(20) DEFAULT NULL,
  PRIMARY KEY (`idclient`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Déchargement des données de la table `client`
--

INSERT INTO `client` (`idclient`, `NomClient`, `Adresse`, `Numtel`) VALUES
('1', 'Christophe', 'Tanambao', '0345672891'),
('C2', 'Rajo', 'Tamatave', '0345567289');

-- --------------------------------------------------------

--
-- Structure de la table `commande`
--

DROP TABLE IF EXISTS `commande`;
CREATE TABLE IF NOT EXISTS `commande` (
  `Idcommande` int NOT NULL,
  `Idclient` varchar(20) DEFAULT NULL,
  `Codepro` varchar(20) DEFAULT NULL,
  `Quantite` int DEFAULT NULL,
  `date` datetime DEFAULT NULL,
  PRIMARY KEY (`Idcommande`),
  KEY `IdClient` (`Idclient`),
  KEY `Codepro` (`Codepro`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Déchargement des données de la table `commande`
--

INSERT INTO `commande` (`Idcommande`, `Idclient`, `Codepro`, `Quantite`, `date`) VALUES
(1, 'C2', 'P001', 20, '2024-08-06 19:27:00'),
(2, '1', 'P002', 20, '2024-08-06 19:28:00'),
(3, '1', 'P001', 13, '2024-08-06 21:47:00'),
(50, '1', 'P001', 20, '2024-08-07 08:11:00'),
(11, '1', 'P001', 7, '2024-08-07 08:15:00'),
(12, '1', 'P001', 3, '2024-08-07 08:15:00');

-- --------------------------------------------------------

--
-- Structure de la table `entrer`
--

DROP TABLE IF EXISTS `entrer`;
CREATE TABLE IF NOT EXISTS `entrer` (
  `idEntrer` int NOT NULL,
  `Codepro` varchar(20) DEFAULT NULL,
  `qt_entre` int DEFAULT NULL,
  `date_entrer` datetime DEFAULT NULL,
  PRIMARY KEY (`idEntrer`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Déchargement des données de la table `entrer`
--

INSERT INTO `entrer` (`idEntrer`, `Codepro`, `qt_entre`, `date_entrer`) VALUES
(1, 'P001', 400, '2024-08-06 19:23:00'),
(2, 'P002', 10002, '2024-08-06 19:23:00');

-- --------------------------------------------------------

--
-- Structure de la table `fournisseur`
--

DROP TABLE IF EXISTS `fournisseur`;
CREATE TABLE IF NOT EXISTS `fournisseur` (
  `NumImm` varchar(20) NOT NULL,
  `NomFourn` varchar(200) DEFAULT NULL,
  `Adresse` varchar(100) DEFAULT NULL,
  `Contact` varchar(20) DEFAULT NULL,
  PRIMARY KEY (`NumImm`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Déchargement des données de la table `fournisseur`
--

INSERT INTO `fournisseur` (`NumImm`, `NomFourn`, `Adresse`, `Contact`) VALUES
('F1', 'fenitra', 'Tanambao', '03400111067'),
('F2', 'kiady', 'Toliara', '03400000067');

-- --------------------------------------------------------

--
-- Structure de la table `produit`
--

DROP TABLE IF EXISTS `produit`;
CREATE TABLE IF NOT EXISTS `produit` (
  `Codepro` varchar(20) NOT NULL,
  `NumImm` varchar(20) DEFAULT NULL,
  `Designation` varchar(100) DEFAULT NULL,
  `Categorie` varchar(100) DEFAULT NULL,
  `Prix_unitaire` int DEFAULT NULL,
  PRIMARY KEY (`Codepro`),
  KEY `NumImm` (`NumImm`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Déchargement des données de la table `produit`
--

INSERT INTO `produit` (`Codepro`, `NumImm`, `Designation`, `Categorie`, `Prix_unitaire`) VALUES
('P001', 'F1', 'Karoti', 'legumes', 3000),
('P002', 'F2', 'pizza', 'delices', 25000),
('P003', 'F1', 'Ovy', 'legumes', 2400);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
