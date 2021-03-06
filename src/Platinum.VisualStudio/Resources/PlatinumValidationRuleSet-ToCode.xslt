﻿<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:p="urn:platinum/validation"
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
    ~ p:exceptions/
    ~
    ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ -->
    <xsl:template match=" p:ruleSet ">
        <xsl:text>// autogenerated: do NOT edit manually
using Platinum;
using Platinum.Validation;
using System;
using System.Collections.Generic;

namespace </xsl:text>

        <xsl:value-of select=" $Namespace " />
        <xsl:text>
{
</xsl:text>
        <xsl:call-template name="p:summary">
            <xsl:with-param name="indent" select=" '    ' " />
        </xsl:call-template>
        <xsl:call-template name="p:remarks">
            <xsl:with-param name="indent" select=" '    ' " />
        </xsl:call-template>
        <xsl:text>    public class </xsl:text>
        <xsl:value-of select=" $FileName " />
        <xsl:text> : IValidationRuleSet
    {
        /// &lt;summary /&gt;
        public IEnumerable&lt;FieldRule&gt; Fields
        {
            get { return _rules.Value; }
        }

        /// &lt;summary /&gt;
        private static Lazy&lt;List&lt;FieldRule&gt;&gt; _rules = new Lazy&lt;List&lt;FieldRule&gt;&gt;( BuildRules );

        /// &lt;summary /&gt;
        private static List&lt;FieldRule&gt; BuildRules()
        {
            List&lt;FieldRule&gt; rs = new List&lt;FieldRule&gt;();</xsl:text>
        <xsl:apply-templates select=" p:field " />
        <xsl:text>

            return rs;
        }
    }
}

/* eof */</xsl:text>
    </xsl:template>


    <!-- ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    ~
    ~ p:field
    ~
    ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ -->
    <xsl:template match=" p:field ">
        <xsl:if test=" count( p:* ) > 0 ">
            <xsl:text>

            rs.Add( new FieldRule()
            {
                Name = "</xsl:text>
            <xsl:value-of select=" @name " />
            <xsl:text>",
                RuleSets = new FieldRuleSet[]
                {</xsl:text>
            <xsl:choose>
                <xsl:when test=" p:if ">
                    <xsl:apply-templates select=" p:if | p:else " mode="p:rule-set" />
                </xsl:when>
                <xsl:otherwise>
                    <xsl:apply-templates select=" . " mode="p:rule-set" />
                </xsl:otherwise>
            </xsl:choose>
            <xsl:text>
                }
            } );</xsl:text>
        </xsl:if>
    </xsl:template>


    <xsl:template match=" node() " mode="p:rule-set">
        <xsl:text>
                    new FieldRuleSet()
                    {
                        Condition = </xsl:text>
        <xsl:apply-templates select=" . " mode="p:condition" />
        <xsl:text>,
                        Rules = new IValidationRule[]
                        {</xsl:text>
        <xsl:apply-templates select=" * " mode="p:rule" />
        <xsl:text>
                        }
                    },</xsl:text>
    </xsl:template>


    <!-- ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    ~
    ~ p:conditions
    ~
    ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ -->
    <xsl:template match=" node() " mode="p:condition">
        <xsl:text>new TrueCondition() /* unsupported */</xsl:text>
    </xsl:template>


    <xsl:template match=" p:field | p:else " mode="p:condition">
        <xsl:text>new TrueCondition()</xsl:text>
    </xsl:template>


    <xsl:template match=" p:if " mode="p:condition">
        <xsl:text>new PropertyIsEqualCondition( "</xsl:text>
        <xsl:value-of select=" @when " />
        <xsl:text>", "</xsl:text>
        <xsl:value-of select=" @equals " />
        <xsl:text>" )</xsl:text>
    </xsl:template>


    <!-- ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    ~
    ~ p:rules
    ~
    ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ -->
    <xsl:template match=" node() " mode="p:rule">
        <xsl:text>
                            /* unsupported: </xsl:text>
        <xsl:value-of select=" local-name(.) " />
        <xsl:text> */</xsl:text>
    </xsl:template>

    <xsl:template match=" p:summary " mode="p:rule" />
    <xsl:template match=" p:remarks " mode="p:rule" />

    <xsl:template match=" p:required " mode="p:rule">
        <xsl:text>
                            new RequiredAttribute(),</xsl:text>
    </xsl:template>

    <xsl:template match=" p:forbidden " mode="p:rule">
        <xsl:text>
                            new ForbiddenAttribute(),</xsl:text>
    </xsl:template>

    <xsl:template match=" p:email " mode="p:rule">
        <xsl:text>
                            new EmailAttribute(),</xsl:text>
    </xsl:template>

    <xsl:template match=" p:function " mode="p:rule">
        <xsl:text>
                            new JavascriptFunctionAttribute( typeof( </xsl:text>
        <xsl:value-of select=" $FileName " />
        <xsl:text> ), "</xsl:text>
        <xsl:value-of select=" @resx " />
        <xsl:text>" )</xsl:text>

        <xsl:if test=" @name ">
            <xsl:text> { Name = "</xsl:text>
            <xsl:value-of select=" @name " />
            <xsl:text>" }</xsl:text>
        </xsl:if>
        
        <xsl:text>,</xsl:text>
    </xsl:template>

    <xsl:template match=" p:regex " mode="p:rule">
        <xsl:text>
                            new RegularExpressionAttribute( @"</xsl:text>
        <xsl:value-of select=" @pattern " />
        <xsl:text>" )</xsl:text>

        <xsl:if test=" @name or @flex = 'true' ">
            <xsl:text> {</xsl:text>

            <xsl:if test=" @name ">
                <xsl:text> Name = "</xsl:text>
                <xsl:value-of select=" @name " />
                <xsl:text>",</xsl:text>
            </xsl:if>

            <xsl:if test=" @flex = 'true' ">
                <xsl:text> Flex = true,</xsl:text>
            </xsl:if>

            <xsl:text> }</xsl:text>
        </xsl:if>

        <xsl:text>,</xsl:text>
    </xsl:template>

    <xsl:template match=" p:length " mode="p:rule">
        <xsl:if test=" @min ">
            <xsl:text>
                            new MinLengthAttribute( </xsl:text>
            <xsl:value-of select=" @min " />
            <xsl:text> ),</xsl:text>
        </xsl:if>

        <xsl:if test=" @max ">
            <xsl:text>
                            new MaxLengthAttribute( </xsl:text>
            <xsl:value-of select=" @max " />
            <xsl:text> ),</xsl:text>
        </xsl:if>
    </xsl:template>

    <xsl:template match=" p:in " mode="p:rule">
        <xsl:text>
                            new InListAttribute( "</xsl:text>
        <xsl:value-of select=" @list " />
        <xsl:text>", "</xsl:text>
        <xsl:choose>
            <xsl:when test=" not( @separator ) ">
                <xsl:text> </xsl:text>
            </xsl:when>
            <xsl:otherwise>
                <xsl:value-of select=" @separator " />
            </xsl:otherwise>
        </xsl:choose>
        <xsl:text>" ),</xsl:text>
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


</xsl:stylesheet>
