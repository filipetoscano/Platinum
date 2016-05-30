<?xml version="1.0" encoding="utf-8" ?>
<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

    <xsl:import href="./world.xslt" />

    <xsl:template match=" node() ">
        <xsl:text>hello</xsl:text>
    </xsl:template>
    
</xsl:stylesheet>