﻿<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:p="urn:platinum/data/statements"
    xmlns:fn="urn:eo-util"
    exclude-result-prefixes="msxsl p fn">

    <xsl:output method="text" indent="no" />

    <xsl:param name="ToolVersion" />
    <xsl:param name="FileName" />
    <xsl:param name="FullFileName" />
    <xsl:param name="Namespace" />

    <xsl:variable name="NewLine">
        <xsl:text>
</xsl:text>
    </xsl:variable>



    <xsl:template match=" p:statements-internal ">
        <xsl:text>// autogenerated: do NOT edit manually
using Platinum.Data;

namespace </xsl:text>
        <xsl:value-of select=" $Namespace "/>
        <xsl:text>
{
    /// &lt;summary /&gt;
    internal static partial class Statements
    {</xsl:text>

        <xsl:apply-templates select=" p:add ">
            <xsl:sort select=" @name " />
        </xsl:apply-templates>
        
        <xsl:text>    }
}</xsl:text>
    </xsl:template>


    <xsl:template match=" p:add ">
        <xsl:param name="indent" select=" '        ' " />

        <xsl:value-of select=" $NewLine" />

        <xsl:value-of select=" $indent " />
        <xsl:text>/// &lt;summary /&gt;</xsl:text>
        <xsl:value-of select=" $NewLine" />

        <xsl:value-of select=" $indent " />
        <xsl:text>internal static partial class </xsl:text>
        <xsl:value-of select=" @name " />
        <xsl:value-of select=" $NewLine" />

        <xsl:value-of select=" $indent " />
        <xsl:text>{</xsl:text>

        <xsl:apply-templates select=" p:add ">
            <xsl:with-param name="indent" select=" concat( $indent, '    ' ) " />
            <xsl:sort select=" @name " />
        </xsl:apply-templates>

        <xsl:apply-templates select=" p:file ">
            <xsl:with-param name="indent" select=" concat( $indent, '    ' ) " />
            <xsl:sort select=" @name " />
        </xsl:apply-templates>

        <xsl:if test=" count( p:add ) = 0 and count( p:file ) = 0 ">
            <xsl:value-of select=" $NewLine" />
        </xsl:if>

        <xsl:value-of select=" $indent " />
        <xsl:text>}</xsl:text>
        <xsl:value-of select=" $NewLine" />
    </xsl:template>



    <xsl:template match=" p:file ">
        <xsl:param name="indent" />

        <xsl:value-of select=" $NewLine" />

        <xsl:value-of select=" $indent " />
        <xsl:text>/// &lt;summary /&gt;</xsl:text>
        <xsl:value-of select=" $NewLine" />

        <xsl:value-of select=" $indent " />
        <xsl:text>internal static string </xsl:text>
        <xsl:value-of select=" @name " />
        <xsl:value-of select=" $NewLine" />

        <xsl:value-of select=" $indent " />
        <xsl:text>{ get { return Db.Command( "</xsl:text>
        <xsl:value-of select=" @resx " />
        <xsl:text>" ); } }</xsl:text>
        <xsl:value-of select=" $NewLine" />
    </xsl:template>

</xsl:stylesheet>
