-- phpMyAdmin SQL Dump
-- version 4.9.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Feb 21, 2020 at 11:20 AM
-- Server version: 10.4.8-MariaDB
-- PHP Version: 7.1.32

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET AUTOCOMMIT = 0;
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `dbproduct_entry`
--

DELIMITER $$
--
-- Procedures
--
CREATE DEFINER=`root`@`localhost` PROCEDURE `Sp_Insert_Product_Entry` (IN `Name` VARCHAR(50), IN `Price` DECIMAL(50), IN `Quantity` INT(10), IN `IsIGSTApplicable` BOOLEAN, IN `Purchase_Date` VARCHAR(50), IN `Expiry_Date` VARCHAR(50), IN `Color` VARCHAR(50))  INSERT INTO tbl_product_entry(Name, Price, Quantity, IsIGSTApplicable, Purchase_Date, Expiry_Date, Color) VALUES ( Name, Price, Quantity, IsIGSTApplicable, Purchase_Date, Expiry_Date, Color)$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `Sp_Select_Product_Entry` (IN `Id` INT(10))  NO SQL
IF Id=0
THEN
SELECT * FROM tbl_product_entry;
ELSE
SELECT ID, Name, Price, Quantity, IsIGSTApplicable, Purchase_Date, Expiry_Date, Color FROM tbl_product_entry WHERE  ID=Id;
END IF$$

DELIMITER ;

-- --------------------------------------------------------

--
-- Table structure for table `tbl_product_entry`
--

CREATE TABLE `tbl_product_entry` (
  `ID` int(10) NOT NULL,
  `Name` varchar(50) NOT NULL,
  `Price` decimal(50,0) NOT NULL,
  `Quantity` int(10) NOT NULL,
  `IsIGSTApplicable` tinyint(1) NOT NULL,
  `Purchase_Date` varchar(50) NOT NULL,
  `Expiry_Date` varchar(50) NOT NULL,
  `Color` varchar(50) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `tbl_product_entry`
--

INSERT INTO `tbl_product_entry` (`ID`, `Name`, `Price`, `Quantity`, `IsIGSTApplicable`, `Purchase_Date`, `Expiry_Date`, `Color`) VALUES
(1, 'Kurti', '600', 1, 1, '2020-02-12', '2020-10-14', 'PINK'),
(2, 'Shirt', '550', 1, 1, '2020-02-12', '2020-10-14', 'RED');

--
-- Indexes for dumped tables
--

--
-- Indexes for table `tbl_product_entry`
--
ALTER TABLE `tbl_product_entry`
  ADD PRIMARY KEY (`ID`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `tbl_product_entry`
--
ALTER TABLE `tbl_product_entry`
  MODIFY `ID` int(10) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
