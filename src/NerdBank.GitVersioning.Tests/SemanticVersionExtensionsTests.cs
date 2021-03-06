﻿using System.Collections.Generic;
using System.Reflection;
using Nerdbank.GitVersioning;
using Xunit;
using Xunit.Sdk;

using static Nerdbank.GitVersioning.VersionOptions;

/// <summary>
/// Tests for <see cref=""/>
/// </summary>
public class SemanticVersionExtensionsTests
{
    [Theory]
    [InlineData("1.0", ReleaseVersionIncrement.Minor, "1.1")]
    [InlineData("1.1", ReleaseVersionIncrement.Minor, "1.2")]
    [InlineData("1.0", ReleaseVersionIncrement.Major, "2.0")]
    [InlineData("1.1", ReleaseVersionIncrement.Major, "2.0")]
    [InlineData("1.0-tag", ReleaseVersionIncrement.Minor, "1.1-tag")]
    [InlineData("1.0-tag", ReleaseVersionIncrement.Major, "2.0-tag")]
    [InlineData("1.0+metadata", ReleaseVersionIncrement.Minor, "1.1+metadata")]
    [InlineData("1.0+metadata", ReleaseVersionIncrement.Major, "2.0+metadata")]
    [InlineData("1.0-tag+metadata", ReleaseVersionIncrement.Minor, "1.1-tag+metadata")]
    [InlineData("1.0-tag+metadata", ReleaseVersionIncrement.Major, "2.0-tag+metadata")]
    [InlineData("1.2.3", ReleaseVersionIncrement.Minor, "1.3.0")]
    [InlineData("1.2.3", ReleaseVersionIncrement.Major, "2.0.0")]
    [InlineData("1.2.3.4", ReleaseVersionIncrement.Minor, "1.3.0.0")]
    [InlineData("1.2.3.4", ReleaseVersionIncrement.Major, "2.0.0.0")]
    public void IncrementVersion(string currentVersionString, ReleaseVersionIncrement increment, string expectedVersionString)
    {
        var currentVersion = SemanticVersion.Parse(currentVersionString);
        var expectedVersion = SemanticVersion.Parse(expectedVersionString);

        var actualVersion = currentVersion.Increment(increment);

        Assert.Equal(expectedVersion, actualVersion);
    }

    [Theory]
    // no prerelease tag in input version
    [InlineData("1.2", "pre", "1.2-pre")]
    [InlineData("1.2", "-pre", "1.2-pre")]
    [InlineData("1.2+build", "pre", "1.2-pre+build")]
    [InlineData("1.2.3", "pre", "1.2.3-pre")]
    [InlineData("1.2.3+build", "pre", "1.2.3-pre+build")]
    // single prerelease tag in input version
    [InlineData("1.2-alpha", "beta", "1.2-beta")]
    [InlineData("1.2-alpha", "-beta", "1.2-beta")]
    [InlineData("1.2.3-alpha", "beta", "1.2.3-beta")]
    [InlineData("1.2-alpha+metadata", "-beta", "1.2-beta+metadata")]
    // multiple prerelease tags
    [InlineData("1.2-alpha.preview", "beta", "1.2-beta.preview")]
    [InlineData("1.2-alpha.preview", "-beta", "1.2-beta.preview")]
    [InlineData("1.2-alpha.preview+metadata", "beta", "1.2-beta.preview+metadata")]    
    [InlineData("1.2.3-alpha.preview", "beta", "1.2.3-beta.preview")]
    [InlineData("1.2-alpha.{height}", "beta", "1.2-beta.{height}")]
    // remove tag
    [InlineData("1.2-pre", "", "1.2")]
    public void SetFirstPrereleaseTag(string currentVersionString, string newTag, string expectedVersionString)
    {
        var currentVersion = SemanticVersion.Parse(currentVersionString);
        var expectedVersion = SemanticVersion.Parse(expectedVersionString);

        var actualVersion = currentVersion.SetFirstPrereleaseTag(newTag);

        Assert.Equal(expectedVersion, actualVersion);
    }
}

