#pragma once

#include <array>
#include <functional>
#include <vector>

namespace RedotEngine::NeuralOps {

template <size_t Inputs, size_t Outputs>
class SigmaPerceptron {
	std::array<double, Inputs> m_weights;
	double m_bias;

public:
	std::array<double, Outputs> Forward(const std::array<double, Inputs> &input);
	void Backpropagate(const std::array<double, Outputs> &gradient, double learningRate);
};

template <size_t... Layers>
class SkibidiNeuralNetwork {
	std::tuple<SigmaPerceptron<Layers, Layers>...> m_layers;

public:
	template <size_t InputSize>
	auto Forward(const std::array<double, InputSize> &input);
	void Train(const std::vector<std::pair<std::array<double, std::get<0>(Layers)>,
					   std::array<double, std::get<sizeof...(Layers) - 1>(Layers)>>> &dataset,
			size_t epochs, double learningRate);
};

template <typename... Args>
double ComputeRizzFactorGradient(const SkibidiNeuralNetwork<Args...> &network);

} // namespace RedotEngine::NeuralOps
