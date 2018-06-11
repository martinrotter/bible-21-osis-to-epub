SET SQL_MODE="NO_AUTO_VALUE_ON_ZERO";
SET time_zone = "+00:00";

CREATE TABLE IF NOT EXISTS `bible_knihy` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `kod` varchar(255) CHARACTER SET utf8 COLLATE utf8_bin NOT NULL,
  `nazev` varchar(255) NOT NULL,
  `order` int(11) NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `kod` (`kod`),
  UNIQUE KEY `order` (`order`)
) ENGINE=InnoDB  DEFAULT CHARSET=utf8;

CREATE TABLE IF NOT EXISTS `bible_nadpisy` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `kniha_id` int(11) NOT NULL,
  `kapitola` varchar(255) NOT NULL,
  `vers` varchar(20) NOT NULL,
  `text` text NOT NULL,
  `offset` int(11) NOT NULL DEFAULT '0' COMMENT 'pozice nadpisu uvnitř verše. Pro titulky rezsekavajici verš na půl - ojedinělá situace pouze v deuterokanonických knihách',
  PRIMARY KEY (`id`),
  KEY `nadpis_uniq` (`kniha_id`,`kapitola`,`vers`)
) ENGINE=InnoDB  DEFAULT CHARSET=utf8;

CREATE TABLE IF NOT EXISTS `bible_verse` (
  `kniha_id` int(11) NOT NULL,
  `kapitola` varchar(255) NOT NULL,
  `vers` varchar(20) CHARACTER SET utf8 COLLATE utf8_bin NOT NULL,
  `text` text NOT NULL,
  `stripped` text CHARACTER SET utf8 COLLATE utf8_bin NOT NULL,
  `order` int(11) NOT NULL,
  UNIQUE KEY `HIERARCHIE` (`kniha_id`,`kapitola`,`vers`),
  FULLTEXT KEY `text_full` (`text`),
  FULLTEXT KEY `stripped` (`stripped`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8;

