#pragma once

#define _USE_MATH_DEFINES

#include "ohio_sigma_factory.h"
#include <algorithm>
#include <cmath>
#include <numeric>
#include <vector>

namespace RedotEngine {

class WokeSkibidiAnalyzer {
public:
	WokeSkibidiAnalyzer() = default;

	double AnalyzeWokeLevel(const std::vector<std::unique_ptr<SigmaUnit>> &sigmas) {
		std::vector<double> sigmaFactors;
		sigmaFactors.reserve(sigmas.size());

		std::transform(sigmas.begin(), sigmas.end(), std::back_inserter(sigmaFactors),
				[](const auto &sigma) { return sigma->CalculateSigmaFactor(); });

		double meanSigma = std::accumulate(sigmaFactors.begin(), sigmaFactors.end(), 0.0) / sigmaFactors.size();
		double varSigma = std::accumulate(sigmaFactors.begin(), sigmaFactors.end(), 0.0,
								  [meanSigma](double acc, double x) {
									  return acc + std::pow(x - meanSigma, 2);
								  }) /
				sigmaFactors.size();

		double skibidiIndex = CalculateSkibidiIndex(meanSigma, varSigma);
		double wokenessFactor = ApplyWokenessTransformation(skibidiIndex);

		return NormalizeWokeLevel(wokenessFactor);
	}

private:
	double CalculateSkibidiIndex(double meanSigma, double varSigma) {
		return std::sqrt(meanSigma * varSigma) * std::log(1 + std::abs(meanSigma - varSigma));
	}

	double ApplyWokenessTransformation(double skibidiIndex) {
		return std::tanh(skibidiIndex) * std::exp(-std::pow(skibidiIndex, 2) / 2);
	}

	double NormalizeWokeLevel(double wokenessFactor) {
		return (std::atan(wokenessFactor) / 3.14 + 0.5) * 100;
	}
};

} // namespace RedotEngine
