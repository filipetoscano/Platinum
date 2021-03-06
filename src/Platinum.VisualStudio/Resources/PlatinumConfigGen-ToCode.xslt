﻿<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:p="urn:platinum/config"
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


    <!-- ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    ~
    ~ p:configuration/
    ~
    ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ -->
    <xsl:template match=" p:configuration ">
        <xsl:text>// autogenerated: do NOT edit manually
using Platinum.Configuration;
using System;
using System.ComponentModel;
using System.Configuration;
using KvConfig = Platinum.Configuration.KeyValueConfigurationElement;
using NullableString = Platinum.Configuration.NullableString;

namespace </xsl:text>
        <xsl:value-of select=" $Namespace " />
        <xsl:text>
{</xsl:text>

        <xsl:apply-templates select=" p:section " mode="p:class" />
        <xsl:apply-templates select=" .//p:item | .//p:element " mode="p:class" />

        <xsl:text>
}

/* eof */
</xsl:text>
    </xsl:template>


    <!-- ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    ~
    ~ p:class/
    ~
    ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ -->
    <xsl:template match=" * " mode="p:class">
        <!-- Lead -->
        <xsl:value-of select=" $NewLine " />
        <xsl:value-of select=" $NewLine " />

        <!-- Decl -->
        <xsl:call-template name="p:summary">
            <xsl:with-param name="indent" select=" '    ' " />
        </xsl:call-template>
        <xsl:call-template name="p:remarks">
            <xsl:with-param name="indent" select=" '    ' " />
        </xsl:call-template>
        <xsl:text>    public partial class </xsl:text>
        <xsl:value-of select=" @type " />
        <xsl:text> : ConfigurationElement</xsl:text>
        <xsl:text>
    {</xsl:text>

        <!-- Properties -->
        <xsl:apply-templates select=" *[ not( local-name(.) = 'summary' or local-name(.) = 'remarks' ) ] " mode="p:property" />

        <xsl:text>    }</xsl:text>
        <xsl:value-of select=" $NewLine " />
    </xsl:template>


    <xsl:template match=" p:section " mode="p:class">
        <!-- Lead -->
        <xsl:value-of select=" $NewLine " />

        <!-- Decl -->
        <xsl:call-template name="p:summary">
            <xsl:with-param name="indent" select=" '    ' " />
        </xsl:call-template>
        <xsl:call-template name="p:remarks">
            <xsl:with-param name="indent" select=" '    ' " />
        </xsl:call-template>
        <xsl:text>    public partial class </xsl:text>
        <xsl:value-of select=" @type " />
        <xsl:text> : ConfigurationSection</xsl:text>
        <xsl:text>
    {
</xsl:text>

        <!-- Singleton -->
        <xsl:text>        /// &lt;summary&gt;</xsl:text>
        <xsl:value-of select=" $NewLine " />
        <xsl:text>        /// Gets the configuration instance for section '</xsl:text>
        <xsl:value-of select=" @name " />
        <xsl:text>'</xsl:text>
        <xsl:value-of select=" $NewLine " />
        <xsl:text>        /// &lt;/summary&gt;</xsl:text>
        <xsl:value-of select=" $NewLine " />
        <xsl:text>        public static </xsl:text>
        <xsl:value-of select=" @type " />
        <xsl:text> Current</xsl:text>

        <xsl:value-of select=" $NewLine " />
        <xsl:text>        {</xsl:text>

        <xsl:value-of select=" $NewLine " />
        <xsl:text>            get { return AppConfiguration.SectionGet&lt;</xsl:text>
        <xsl:value-of select=" @type " />
        <xsl:text>&gt;( "</xsl:text>
        <xsl:value-of select=" @name " />
        <xsl:text>" ); }</xsl:text>

        <xsl:value-of select=" $NewLine " />
        <xsl:text>        }</xsl:text>
        <xsl:value-of select=" $NewLine " />

        <!-- Properties -->
        <xsl:apply-templates select=" *[ not( local-name(.) = 'summary' or local-name(.) = 'remarks' ) ] " mode="p:property" />
        <xsl:text>    }</xsl:text>
        <xsl:value-of select=" $NewLine " />
    </xsl:template>


    <!-- ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    ~
    ~ p:property/
    ~ For all properties, and a specialized generation for the repeater.
    ~
    ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ -->
    <xsl:template match=" * " mode="p:property">
        <xsl:value-of select=" $NewLine " />
        <xsl:value-of select=" $NewLine " />

        <!-- ConfigurationProperty -->
        <xsl:call-template name="p:summary" />
        <xsl:call-template name="p:remarks" />
        <xsl:text>        [ConfigurationProperty( "</xsl:text>
        <xsl:value-of select=" @name " />
        <xsl:text>"</xsl:text>

        <xsl:if test=" @key = 'true' ">
            <xsl:text>, IsKey = true, IsRequired = true</xsl:text>
        </xsl:if>

        <xsl:if test=" not( @optional = 'true' ) and not( @key = 'true' ) ">
            <xsl:text>, IsRequired = true</xsl:text>
        </xsl:if>

        <xsl:if test=" @default ">
            <xsl:text>, DefaultValue = </xsl:text>
            <xsl:apply-templates select=" . " mode="p:default" />
        </xsl:if>

        <xsl:text> )]</xsl:text>

        <!-- Hack! -->
        <xsl:if test=" local-name(.) = 'string' and @optional = 'true' and not( @default ) ">
            <xsl:value-of select=" $NewLine " />
            <xsl:text>        </xsl:text>
            <xsl:text>[TypeConverter( typeof( NullableStringConverter ) )]</xsl:text>
        </xsl:if>

        <!-- Validators -->
        <xsl:apply-templates select=" * " mode="p:validator" />

        <!-- Definition -->
        <xsl:value-of select=" $NewLine " />
        <xsl:text>        public </xsl:text>
        <xsl:apply-templates select=" . " mode="p:type" />
        <xsl:text> </xsl:text>
        <xsl:choose>
            <xsl:when test=" @as ">
                <xsl:value-of select=" @as " />
            </xsl:when>
            <xsl:otherwise>
                <xsl:call-template name="p:capitalize">
                    <xsl:with-param name="value" select=" @name " />
                </xsl:call-template>
            </xsl:otherwise>
        </xsl:choose>
        <xsl:text>
        {</xsl:text>

        <xsl:text>
            get { return (</xsl:text>
        <xsl:apply-templates select=" . " mode="p:type" />
        <xsl:text>) this[ "</xsl:text>
        <xsl:value-of select=" @name "/>
        <xsl:text>" ]; }</xsl:text>

        <xsl:text>
            set { this[ "</xsl:text>
        <xsl:value-of select=" @name "/>
        <xsl:text>" ] = value; }</xsl:text>

        <xsl:text>
        }</xsl:text>
        <xsl:value-of select=" $NewLine " />
    </xsl:template>


    <xsl:template match=" p:repeater " mode="p:property">
        <xsl:value-of select=" $NewLine " />
        <xsl:value-of select=" $NewLine " />

        <!-- ConfigurationProperty -->
        <xsl:call-template name="p:summary" />
        <xsl:call-template name="p:remarks" />
        <xsl:text>        [ConfigurationProperty( "</xsl:text>
        <xsl:value-of select=" @name " />
        <xsl:text>", IsDefaultCollection = false )]</xsl:text>

        <!-- ConfigurationCollection -->
        <xsl:value-of select=" $NewLine " />
        <xsl:text>        [ConfigurationCollection( typeof( </xsl:text>
        <xsl:value-of select=" p:item/@type " />
        <xsl:text> ), AddItemName = "</xsl:text>
        <xsl:value-of select=" p:item/@name " />
        <xsl:text>" )]</xsl:text>

        <!-- Type -->
        <xsl:value-of select=" $NewLine " />
        <xsl:text>        public ConfigurationElementCollection&lt;</xsl:text>
        <xsl:value-of select=" p:item/@type " />
        <xsl:text>&gt; </xsl:text>
        <xsl:choose>
            <xsl:when test=" @as ">
                <xsl:value-of select=" @as " />
            </xsl:when>
            <xsl:otherwise>
                <xsl:call-template name="p:capitalize">
                    <xsl:with-param name="value" select=" @name " />
                </xsl:call-template>
            </xsl:otherwise>
        </xsl:choose>

        <!-- Open get/setter -->
        <xsl:value-of select=" $NewLine " />
        <xsl:text>        {</xsl:text>

        <!-- Getter -->
        <xsl:value-of select=" $NewLine " />
        <xsl:text>            get { return (ConfigurationElementCollection&lt;</xsl:text>
        <xsl:value-of select=" p:item/@type " />
        <xsl:text>&gt;) base[ "</xsl:text>
        <xsl:value-of select=" @name " />
        <xsl:text>" ]; }</xsl:text>

        <!-- Close -->
        <xsl:value-of select=" $NewLine " />
        <xsl:text>        }</xsl:text>
        <xsl:value-of select=" $NewLine " />
    </xsl:template>


    <xsl:template match=" p:keyValue " mode="p:property">
        <xsl:value-of select=" $NewLine " />
        <xsl:value-of select=" $NewLine " />

        <!-- ConfigurationProperty -->
        <xsl:call-template name="p:summary" />
        <xsl:call-template name="p:remarks" />
        <xsl:text>        [ConfigurationProperty( "", IsDefaultCollection = true )]</xsl:text>

        <!-- ConfigurationCollection -->
        <xsl:value-of select=" $NewLine " />
        <xsl:text>        [ConfigurationCollection( typeof( KvConfig ), AddItemName = "add" )]</xsl:text>

        <!-- Type -->
        <xsl:value-of select=" $NewLine " />
        <xsl:text>        public ConfigurationElementCollection&lt;KvConfig&gt; </xsl:text>
        <xsl:choose>
            <xsl:when test=" @as ">
                <xsl:value-of select=" @as " />
            </xsl:when>
            <xsl:otherwise>
                <xsl:text>Settings</xsl:text>
            </xsl:otherwise>
        </xsl:choose>

        <!-- Open get/setter -->
        <xsl:value-of select=" $NewLine " />
        <xsl:text>        {</xsl:text>

        <!-- Getter -->
        <xsl:value-of select=" $NewLine " />
        <xsl:text>            get { return (ConfigurationElementCollection&lt;KvConfig&gt;) base[ "" ]; }</xsl:text>

        <!-- Close -->
        <xsl:value-of select=" $NewLine " />
        <xsl:text>        }</xsl:text>
        <xsl:value-of select=" $NewLine " />
    </xsl:template>


    <!-- ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    ~
    ~ p:type/
    ~
    ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ -->
    <xsl:template match=" * " mode="p:type">
        <xsl:choose>
            <xsl:when test=" local-name(.) = 'string' and @optional = 'true' and not( @default ) ">
                <xsl:text>NullableString</xsl:text>
            </xsl:when>

            <xsl:when test=" local-name(.) = 'string' ">
                <xsl:text>string</xsl:text>
            </xsl:when>

            <xsl:when test=" local-name(.) = 'bool' ">
                <xsl:text>bool</xsl:text>

                <xsl:if test=" @optional = 'true' and not( @default ) ">
                    <xsl:text>?</xsl:text>
                </xsl:if>
            </xsl:when>

            <xsl:when test=" local-name(.) = 'int' ">
                <xsl:text>int</xsl:text>

                <xsl:if test=" @optional = 'true' and not( @default ) ">
                    <xsl:text>?</xsl:text>
                </xsl:if>
            </xsl:when>

            <xsl:when test=" local-name(.) = 'enum' ">
                <xsl:value-of select=" @type " />

                <xsl:if test=" @optional = 'true' and not( @default ) ">
                    <xsl:text>?</xsl:text>
                </xsl:if>
            </xsl:when>

            <xsl:when test=" local-name(.) = 'element' ">
                <xsl:value-of select=" @type " />
            </xsl:when>

            <xsl:otherwise>
                <xsl:text>UNSUPPORTED /* </xsl:text>
                <xsl:value-of select=" local-name(.) " />
                <xsl:text> */</xsl:text>
            </xsl:otherwise>
        </xsl:choose>
    </xsl:template>


    <!-- ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    ~
    ~ p:default/
    ~
    ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ -->
    <xsl:template match=" * " mode="p:default">
        <xsl:value-of select=" @default " />
    </xsl:template>

    <xsl:template match=" p:enum | p:string " mode="p:default">
        <xsl:text>"</xsl:text>
        <xsl:value-of select=" @default " />
        <xsl:text>"</xsl:text>
    </xsl:template>


    <!-- ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    ~
    ~ p:validator/
    ~
    ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ -->
    <xsl:template match=" * " mode="p:validator" />

    <xsl:template match=" p:regex " mode="p:validator">
        <xsl:text>
</xsl:text>
        <xsl:text>        [RegexStringValidator( @"</xsl:text>
        <xsl:value-of select=" @pattern " />
        <xsl:text>" )]</xsl:text>
    </xsl:template>



    <!-- ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    ~
    ~ p:summary/
    ~ p:remarks/
    ~
    ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ -->
    <xsl:template name="p:summary">
        <xsl:param name="indent" select=" '        ' " />

        <xsl:choose>
            <xsl:when test=" p:summary ">
                <xsl:value-of select=" $indent " />
                <xsl:text>/// &lt;summary&gt;</xsl:text>
                <!-- This new line not needed! -->

                <xsl:value-of select=" fn:ToWrap( p:summary/text(), concat( $indent, '/// ' ), 80 ) "/>
                <xsl:value-of select=" $NewLine " />

                <xsl:value-of select=" $indent " />
                <xsl:text>/// &lt;/summary&gt;</xsl:text>
            </xsl:when>
            <xsl:otherwise>
                <xsl:value-of select=" $indent " />
                <xsl:text>/// &lt;summary /&gt;</xsl:text>
            </xsl:otherwise>
        </xsl:choose>
        <xsl:value-of select=" $NewLine " />
    </xsl:template>

    <xsl:template name="p:remarks">
        <xsl:param name="indent" select=" '        ' " />

        <xsl:if test=" p:remarks ">
            <xsl:value-of select=" $indent " />
            <xsl:text>/// &lt;remarks&gt;</xsl:text>
            <!-- This new line not needed! -->

            <xsl:value-of select=" fn:ToWrap( p:remarks/text(), concat( $indent, '/// ' ), 80 ) "/>
            <xsl:value-of select=" $NewLine " />

            <xsl:value-of select=" $indent " />
            <xsl:text>/// &lt;/remarks&gt;</xsl:text>
            <xsl:value-of select=" $NewLine " />
        </xsl:if>
    </xsl:template>


    <!-- ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    ~
    ~ p:capitalize()
    ~
    ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ -->
    <xsl:variable name="lcase" select=" 'abcdefghijklmnopqrstuvwxyz' " />
    <xsl:variable name="ucase" select=" 'ABCDEFGHIJKLMNOPQRSTUVWXYZ' " />

    <xsl:template name="p:capitalize">
        <xsl:param name="value" />

        <xsl:value-of select=" translate( substring( $value, 1, 1 ), $lcase, $ucase ) " />
        <xsl:value-of select=" substring( $value, 2 ) " />
    </xsl:template>

</xsl:stylesheet>
